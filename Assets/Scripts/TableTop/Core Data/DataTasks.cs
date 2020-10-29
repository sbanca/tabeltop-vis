using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace TableTop
{

    [Serializable]
    public class TasksDataGroups
    {
        public List<TasksData> List;

    }

    [Serializable]
    public class TasksData
    {
        public List<TaskData> List;       
        public UserID User;
        public string Title;
        public float[] Position;
        public float[] Scale;
        public float[] Rotation;
        public PanelType Type;      

        public async Task Update()
        {

            InitializeSelectionsAndMetrics();
            //await UpdateRouteData();
            CreateUiElementList();
            UpdateRoutesMetrics();
            
        }

        public void InitializeSelectionsAndMetrics()
        {

            lock (List)
            {

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {
                    for (int i = 0; i < List.Count; i++)
                    {

                        for (int j = 0; j < this.List[i].Options.Count; j++)
                        {

                            if (j == this.List[i].SelectedOption) this.List[i].Options[j].Selected = true;

                            else this.List[i].Options[j].Selected = false;

                        }

                    }
                }
                else
                {

                    for (int i = 0; i < List.Count; i++)
                    {

                        this.List[i].TimeDifferenceInSeconds = 0;

                        this.List[i].OriginName = this.Title;

                    }
                }
            }
        }


      

        //metrics
        public int StartTime;
        public int totalDuration;
        public int routeDuration;
        public int routeDistance;
        public int routeDelay;

        private void UpdateRoutesMetrics()
        {

            lock (List) {

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    totalDuration = StartTime;

                    routeDuration = 0;

                    routeDelay = 0;

                    routeDistance = 0;

                    foreach (UiItem item in UiItemList)
                    {


                        switch (item.type)
                        {



                            case (UiItemType.ADDEDTASK):

                                //delay 
                                if (item.taskData.TimeLocked) item.taskData.TimeDifferenceInSeconds = totalDuration - item.taskData.TimeInSeconds;

                                if (item.taskData.TimeDifferenceInSeconds > 0) routeDelay += item.taskData.TimeDifferenceInSeconds;
                                //end delay

                                totalDuration += (int)item.taskData.Duration;

                                break;


                        }


                    }


                    foreach (UiItem item in UiItemList)
                    {


                        switch (item.type)
                        {



                            case (UiItemType.METRICS):

                                item.metricsData = new MetricsData(routeDistance, routeDuration, routeDelay);

                                break;

                        }

                    }

                }

            }
        
        }



        //pannel ui creation
        public List<UiItem> UiItemList;

        public void DisplayRoutes()
        {
            lock (List)
            {
               
            }

        }

        public void CreateUiElementList()
        {

            lock (List)
            {
                UiItemList = new List<UiItem>();

                if (Type == PanelType.TASKASSEMBLYPANNEL)
                {

                    UiItemList.Add(new UiItem(UiItemType.METRICS, new MetricsData(0, 0, 0)));


                    for (int i = 0; i < List.Count; i++)
                    {

                        UiItemList.Add(new UiItem(UiItemType.ADDEDTASK, List[i]));

                       

                    }



                }
                else
                {

                    foreach (TaskData task in List) UiItemList.Add(new UiItem(UiItemType.TASK, task));

                }

                for (int i = 0; i < UiItemList.Count; i++) UiItemList[i].itemNumber = i;
            }
        }



        //get extract add remove tasks 
        public TaskData GetTask(string name)
        {

            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].Name == name) return List[i];

            }

            return null;

        }

        public TaskData ExtractTask(string name)
        {

            TaskData extractedTask = GetTask(name);

            if (extractedTask != null) RemoveTask(extractedTask);

            return extractedTask;
        }

        public void RemoveTask(TaskData extractedTask)
        {
            lock (List)
            {
                string name = extractedTask.Name;

                for (int i = 0; i < List.Count; i++)
                {

                    if (List[i].Name == name)
                    {

                        List.Remove(List[i]);

                        break;
                    }

                }
            }

        }

        public void AddTask(TaskData task)
        {

            lock (List)
            {
                List.Add(task); //add the task
            }

        }
    
    }

    [Serializable]
    public class TaskData
    {
        public string Name;
        public string Description;
        public bool Clickable;
        public List<OptionData> Options;
        public bool TimeLocked;
        public int TimeInSeconds;
        public int Duration; //duration in seconds
        public int SelectedOption;
        public string RouteSegment;

        public string OriginName;

        public int TimeDifferenceInSeconds;

        public OptionData returnSelectedOption()
        {
            OptionData selectedOption = null;

            for (int j = 0; j < this.Options.Count; j++)
            {

                if (j == this.SelectedOption)
                {
                    selectedOption = this.Options[j];
                    break;
                }
            }

            return selectedOption;

        }



    }

    [Serializable]
    public class OptionData
    {
        public string Name;
        public string Description;
        public double Lng;
        public double Lat;
        public Vector3 LocalPos;
        public SpatialAnchorType Type;
        public int number;
        public bool Selected;
        public string RouteSegment;

    }

    public class MetricsData {

        public int totalDistance;
        public int totalDurationInSeconds;
        public int totalDelayInSeconds;

        public MetricsData(int distance, int duration, int delay) {

            this.totalDistance = distance;
            this.totalDurationInSeconds = duration;
            this.totalDelayInSeconds = delay;
        }

    }

    public class UiItem
    {

        public UiItemType type;
       
        public TaskData taskData;
        public MetricsData metricsData;
        public int itemNumber;

        public UiItem(UiItemType type, object Data)
        {

            this.type = type;

            switch (type)
            {

              

                case (UiItemType.TASK):

                    this.taskData = (TaskData)Data;

                    break;

                case (UiItemType.ADDEDTASK):

                    this.taskData = (TaskData)Data;

                    break;

                case (UiItemType.METRICS):

                    this.metricsData = (MetricsData)Data;

                    break;
            }

        }

    }



}