namespace DictationAssistant
{
    static class Module1
    {
        public static string CiZuZengQiangMuLu;

        public static int GetChineseEngineLevel(string Name)
        {
            switch (Name)
            {
                case "VW Lily":
                    {
                        return 5;
                    }

                case "VW Hui":
                    {
                        return 4;
                    }

                case "VW Liang":
                    {
                        return 3;
                    }

                case "VW Wang":
                    {
                        return 2;
                    }

                case "Girl XiaoKun":
                    {
                        return 1;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }
        public static int GetEnglishEngineLevel(string Name)
        {
            switch (Name)
            {
                case "Microsoft Hazel Desktop":
                    {
                        return 4;
                    }

                case "VW Julie":
                    {
                        return 3;
                    }

                case "VW Paul":
                    {
                        return 2;
                    }

                case "VW Kate":
                    {
                        return 1;
                    }

                default:
                    {
                        return 0;
                    }
            }
        }
    }
}
