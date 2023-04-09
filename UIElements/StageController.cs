using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingOfCartridges.UIElements
{
    class StageController
    {
        List<Stage> Stages = new List<Stage>();

        public void AddStage(Button button, Label feedback) //Добавление этапа
        {
            Stages.Add(new Stage(button, feedback));
            
            //Добавление связей предыдущей стадии с новой.
            int count = Stages.Count;
            Stages[count - 1].StageNum = count;
            if (count > 1) Stages[count - 2].NextStage = Stages[count-1];
        }

        public bool Click(Button button, ref string error)
        {
            Stage current = null;
            foreach(Stage stage in Stages)
            {
                if(stage.Button == button)
                {
                    current = stage;
                }
            }
            bool result = current.Click(ref error);

            if (result && Stages[Stages.Count - 1] == current) ResetStages();

            return result;
        }

        public void ResetStages()
        {
            Stages[0].Reset();
            Stages[0].Interactive(true);

            for(int i = 1; i < Stages.Count; i++)
            {
                Stages[i].Reset();
            }
        }

        public void SetStage(int num)
        {
            ResetStages();

            for (int i = 0; i < num; i++)
            {
                Stages[i].Active(false);
            }
        }
    }
}
