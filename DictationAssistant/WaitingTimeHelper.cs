namespace DictationAssistant
{
    public class WaitingTimeHelper
    {
        /// <summary>
    /// 取单词数（英文）
    /// </summary>
    /// <param name="words">词组</param>
    /// <returns>单词数</returns>
    /// <remarks></remarks>
        public static int GetWordCount(string words)
        {
            string separators = ".,:;?!- "; // 分隔符
            int lastSeparator = 0;
            int count = 1; // 数量
            for (int i = 0; i <= words.Length - 1; i++)
            {
                if (separators.IndexOf(words[i]) != -1)
                {
                    if (!(lastSeparator == i - 1))
                    {
                        if (!(lastSeparator == i - 2 && words[lastSeparator] == '\''))
                            count += 1;
                    }
                    lastSeparator = i;
                }
            }
            if (lastSeparator == words.Length - 1)
                count -= 1;
            return count;
        }
    }
}
