Unicode true

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "自动默写"
!define PRODUCT_VERSION "3.0β"
!define PRODUCT_PUBLISHER "qiqiworld"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\opusenc.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

!include "MUI.nsh"
!include "x64.nsh"
!include "DotNetChecker.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Language Selection Dialog Settings
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; License page
!insertmacro MUI_PAGE_LICENSE ".\DictationAssistant\Resources\CopyrightNotice.txt"
; Directory page
!insertmacro MUI_PAGE_DIRECTORY
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!define MUI_FINISHPAGE_RUN "$INSTDIR\DictationAssistant.exe"
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "SimpChinese"

; Reserve files
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS

; MUI end ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "DictationAssistant-Installer.exe"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show
ManifestDPIAware true
ManifestLongPathAware true

Function .onInit
  ${If} ${RunningX64}
    ${DisableX64FSRedirection}
    StrCpy $INSTDIR "$PROGRAMFILES64\自动默写"
  ${Else}
    StrCpy $INSTDIR "$PROGRAMFILES\自动默写"
  ${EndIf}
  !insertmacro CheckNetFramework 40Client
FunctionEnd

Section "MainSection"
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File ".\DictationAssistant\bin\x86\Release\System.Windows.Forms.ProgressDialog.dll"
  File ".\DictationAssistant\bin\x86\Release\SDL2.dll"
  File ".\DictationAssistant\bin\x86\Release\QIQI.WpfFontPicker.dll"
  File ".\DictationAssistant\bin\x86\Release\NCalc.dll"
  File ".\DictationAssistant\bin\x86\Release\ICSharpCode.AvalonEdit.dll"
  File ".\DictationAssistant\bin\x86\Release\DynamicExpresso.Core.dll"
  File ".\DictationAssistant\bin\x86\Release\CalcBinding.dll"
  File ".\DictationAssistant\bin\x86\Release\BetterFolderBrowser.dll"
  File ".\DictationAssistant\bin\x86\Release\bass.dll"
  File ".\DictationAssistant\bin\x86\Release\Antlr3.Runtime.dll"
${If} ${RunningX64}
  SetOutPath "$INSTDIR"
  File ".\DictationAssistant\bin\x64\Release\SDL2.dll"
  File ".\DictationAssistant\bin\x64\Release\DictationAssistant.exe"
  File ".\DictationAssistant\bin\x64\Release\DictationAssistant.exe.config"
  File ".\DictationAssistant\bin\x64\Release\bass.dll"
  SetOutPath "$INSTDIR\encoder"
  File ".\DictationAssistant\bin\x64\Release\encoder\opusenc.exe"
  File ".\DictationAssistant\bin\x64\Release\encoder\lame.exe"
  SetOutPath "$INSTDIR\bass_plugin"
  File ".\DictationAssistant\bin\x64\Release\bass_plugin\bassopus.dll"
  File ".\DictationAssistant\bin\x64\Release\bass_plugin\bassflac.dll"
  File ".\DictationAssistant\bin\x64\Release\bass_plugin\bassalac.dll"
  File ".\DictationAssistant\bin\x64\Release\bass_plugin\bass_ape.dll"
  File ".\DictationAssistant\bin\x64\Release\bass_plugin\bass_aac.dll"
${Else}
  SetOutPath "$INSTDIR"
  File ".\DictationAssistant\bin\x86\Release\SDL2.dll"
  File ".\DictationAssistant\bin\x86\Release\DictationAssistant.exe"
  File ".\DictationAssistant\bin\x86\Release\DictationAssistant.exe.config"
  File ".\DictationAssistant\bin\x86\Release\bass.dll"
  SetOutPath "$INSTDIR\encoder"
  File ".\DictationAssistant\bin\x86\Release\encoder\opusenc.exe"
  File ".\DictationAssistant\bin\x86\Release\encoder\lame.exe"
  SetOutPath "$INSTDIR\bass_plugin"
  File ".\DictationAssistant\bin\x86\Release\bass_plugin\bassopus.dll"
  File ".\DictationAssistant\bin\x86\Release\bass_plugin\bassflac.dll"
  File ".\DictationAssistant\bin\x86\Release\bass_plugin\bassalac.dll"
  File ".\DictationAssistant\bin\x86\Release\bass_plugin\bass_ape.dll"
  File ".\DictationAssistant\bin\x86\Release\bass_plugin\bass_aac.dll"
${EndIf}
  CreateDirectory "$SMPROGRAMS\自动默写"
  CreateShortCut "$SMPROGRAMS\自动默写\自动默写.lnk" "$INSTDIR\DictationAssistant.exe"
  CreateShortCut "$DESKTOP\自动默写.lnk" "$INSTDIR\DictationAssistant.exe"
SectionEnd

Section -AdditionalIcons
  SetOutPath $INSTDIR
  CreateShortCut "$SMPROGRAMS\自动默写\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\encoder\opusenc.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\encoder\opusenc.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从你的计算机移除。"
FunctionEnd

Function un.onInit
!insertmacro MUI_UNGETLANGUAGE
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "你确实要完全移除 $(^Name) ，其及所有的组件？" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\bass.dll"
  Delete "$INSTDIR\DictationAssistant.exe"
  Delete "$INSTDIR\SDL2.dll"
  Delete "$INSTDIR\encoder\lame.exe"
  Delete "$INSTDIR\encoder\opusenc.exe"
  Delete "$INSTDIR\bass_plugin\bass_aac.dll"
  Delete "$INSTDIR\bass_plugin\bass_ape.dll"
  Delete "$INSTDIR\bass_plugin\bassalac.dll"
  Delete "$INSTDIR\bass_plugin\bassflac.dll"
  Delete "$INSTDIR\bass_plugin\bassopus.dll"
  Delete "$INSTDIR\Antlr3.Runtime.dll"
  Delete "$INSTDIR\bass.dll"
  Delete "$INSTDIR\BetterFolderBrowser.dll"
  Delete "$INSTDIR\CalcBinding.dll"
  Delete "$INSTDIR\DynamicExpresso.Core.dll"
  Delete "$INSTDIR\ICSharpCode.AvalonEdit.dll"
  Delete "$INSTDIR\NCalc.dll"
  Delete "$INSTDIR\QIQI.WpfFontPicker.dll"
  Delete "$INSTDIR\SDL2.dll"
  Delete "$INSTDIR\System.Windows.Forms.ProgressDialog.dll"

  Delete "$SMPROGRAMS\自动默写\Uninstall.lnk"
  Delete "$DESKTOP\自动默写.lnk"
  Delete "$SMPROGRAMS\自动默写\自动默写.lnk"

  RMDir "$SMPROGRAMS\自动默写"
  RMDir "$INSTDIR\encoder"
  RMDir "$INSTDIR\bass_plugin"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd