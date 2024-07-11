
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public bool FirstClick = false;
        public float currentMoney = 0;                       // Можно задать полям значения по умолчанию
        public int amountOfMoneyPerClick = 1;
        public int amountOfMoneyPerAutoClick = 1;
        public bool[] ActiveButtonAuto = new bool[5];
        public bool[] ActiveButtonClick = new bool[5];
        public int[] LevelUpgradeAuto = new int[5];
        public int[] LevelUpgradeClick = new int[5];
        public int[] CostAuto = new int[5];
        public int[] CostClick = new int[5];
        public bool music = true;


        public int money = 0;
        public string newPlayerName = "";
        public bool[] openLevels;
        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива
            for (int i = 0; i < 5; i++)
            {
                ActiveButtonAuto[i] = false;
                ActiveButtonClick[i] = false;
                LevelUpgradeAuto[i] = 0;
                LevelUpgradeClick[i] = 0;
                CostAuto[i] = 0;
                CostClick[i] = 0;
            }          

        }
    }
}
