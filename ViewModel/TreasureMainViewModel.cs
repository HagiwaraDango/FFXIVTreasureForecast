using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TreasureMakerV2.Enum;
using TreasureMakerV2.Model;

namespace TreasureMakerV2.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TreasureMainViewModel : ViewModelBase
    {
        #region private

        private string _mypath = @"D:\TreasureHistory.json";

        private string _leftDoor = "左边";

        private string _rightDoor = "右边";

        private string _bloomingDoor = "发光";

        private DispatcherTimer _dispatcherTimer;

        private DateTime _startTime;
        
        #endregion



        #region Property

       
        /// <summary>
        /// 当前梦羽宝境
        /// </summary>
        public ReactiveProperty<TreasureMapModel> TreasureMapProperty { get; set; }

        /// <summary>
        /// 预测的门
        /// </summary>
        public ReactiveProperty<string> CorrectDoorTextProperty { get; set; }

        /// <summary>
        /// 反转时长
        /// </summary>
        public ReactiveProperty<string> InvertTimeProperty { get; set; }

        /// <summary>
        /// 下一层层数
        /// </summary>
        public ReactiveProperty<int> NextFloorProperty { get; set; }

        /// <summary>
        /// 怪物名1
        /// </summary>
        public ReactiveProperty<string> MonsterNameOneProperty { get; set; }

        /// <summary>
        /// 怪物名2
        /// </summary>
        public ReactiveProperty<string> MonsterNameTwoProperty { get; set; }

        /// <summary>
        /// 选中的怪物
        /// True：左
        /// False：右
        /// </summary>
        public ReactiveProperty<bool> SelectedMonsterProperty { get; set; }

        /// <summary>
        /// 怪物名2
        /// </summary>
        public ReactiveProperty<string> HistoryPathProperty { get; set; }
        #endregion

        #region Collection
        /// <summary>
        /// 梦羽宝境记录
        /// </summary>
        public ObservableCollection<TreasureMapModel> TreasureHistoryCollection { get; set; } = new ObservableCollection<TreasureMapModel>();

        #endregion

        #region Command
        
        public ReactiveCommand CheckButtonCommand { get; set; }

        public ReactiveCommand OpenFileCommand { get; set; }

        public ReactiveCommand InvertResultCommand { get; set; }

        public ReactiveCommand NextFloorCommand { get; set; }

        public ReactiveCommand ResetFloorCommand { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TreasureMainViewModel class.
        /// </summary>
        public TreasureMainViewModel()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            HistoryPathProperty=new ReactiveProperty<string>(_mypath);

            CorrectDoorTextProperty =new ReactiveProperty<string>();

            SelectedMonsterProperty =new ReactiveProperty<bool>(true);

            MonsterNameTwoProperty =new ReactiveProperty<string>();

            MonsterNameOneProperty =new ReactiveProperty<string>();

            InvertTimeProperty =new ReactiveProperty<string>();

            NextFloorProperty=new ReactiveProperty<int>(2);

            TreasureMapProperty = new ReactiveProperty<TreasureMapModel> {Value = new TreasureMapModel()};

            CheckButtonCommand =new ReactiveCommand();

            InvertResultCommand=new ReactiveCommand();

            NextFloorCommand=new ReactiveCommand();

            ResetFloorCommand=new ReactiveCommand();

            OpenFileCommand=new ReactiveCommand();

            OpenFileCommand.Subscribe(OpenHistory);

            CheckButtonCommand.Subscribe(CheckStatus);

            InvertResultCommand.Subscribe(InvertStatusControl);

            ResetFloorCommand.Subscribe(ResetFloor);

            NextFloorCommand.Subscribe(NextFloor);

            SetMonster(1);

            CheckStatus();

            GetJsonFile(_mypath);
        }

        /// <summary>
        ///  选项检测
        /// </summary>
        /// <param name="type"></param>
        public void CheckStatus()
        {
            switch (TreasureMapProperty.Value.NowFloor)
            {
                case 1:
                    TreasureMapProperty.Value.MonsterType = SelectedMonsterProperty.Value ? FFXIVEnum.MonsterTypeEnum.Dragon : FFXIVEnum.MonsterTypeEnum.Tree;
                    break;
                case 2:
                    TreasureMapProperty.Value.MonsterType = FFXIVEnum.MonsterTypeEnum.Bear;
                    break;
                case 3:
                    TreasureMapProperty.Value.MonsterType = SelectedMonsterProperty.Value ? FFXIVEnum.MonsterTypeEnum.ButterFly : FFXIVEnum.MonsterTypeEnum.Moth;
                    break;
                case 4:
                    TreasureMapProperty.Value.MonsterType = SelectedMonsterProperty.Value ? FFXIVEnum.MonsterTypeEnum.DragonBird : FFXIVEnum.MonsterTypeEnum.Ghost;

                    break;
            }
         
            var result = 0;

            //0:left 1:right
            var door = 0;
            
            if (!TreasureMapProperty.Value.IsBloom)
            {
                //if (TreasureMapProperty.Value.HaveMandra)
                //    result += 0;
                //if (TreasureMapProperty.Value.HaveWaterWitcher)
                //    result += 0;
                if (TreasureMapProperty.Value.HaveTrap)
                    result += 1;
                if (TreasureMapProperty.Value.HaveDoorKeeper)
                    result += 1;

                result %= 2;

               switch (TreasureMapProperty.Value.MonsterType)
                {
                    case FFXIVEnum.MonsterTypeEnum.Bear:
                        door = result != 0 ? 1 : 0;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.ButterFly:
                        door = result != 0 ? 1 : 0;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.Dragon:
                        door = result != 0 ? 1 : 0;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.DragonBird:
                        door = result != 0 ? 1 : 0;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.Ghost:
                        door = result != 0 ? 0 : 1;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.Moth:
                        door = result != 0 ? 0 : 1;
                        break;
                    case FFXIVEnum.MonsterTypeEnum.Tree:
                        door = result != 0 ? 0 : 1;
                        break;
                }

               if (TreasureMapProperty.Value.IsInvert)
               {
                   door = door == 0 ? 1 : 0;
               }

                switch (door)
                {
                    case 0:
                        CorrectDoorTextProperty.Value = _leftDoor;
                        break;
                    case 1:
                        CorrectDoorTextProperty.Value = _rightDoor;
                        break;
                }
            }
            else
                CorrectDoorTextProperty.Value = _bloomingDoor;

        }

        /// <summary>
        /// 反转控制
        /// </summary>
        public void InvertStatusControl()
        {

            if (TreasureMapProperty.Value.IsInvert)
            {
                _dispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 1, 0) };
                _dispatcherTimer.Tick += t_Tick;
                _dispatcherTimer.Start();
                _startTime=DateTime.Now;
            }
            else
            {
                _dispatcherTimer.Stop();
                InvertTimeProperty.Value=string.Empty;
            }

            CheckStatus();


        }
       
        private void t_Tick(object sender, EventArgs e)
        {
            var newTime = (DateTime.Now - _startTime);
            InvertTimeProperty.Value = $"{newTime.Minutes}分";
        }

        private void SetMonster(int floor)
        {
            switch (floor)
            {
                case 1:
                    
                    MonsterNameOneProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.Dragon);

                    MonsterNameTwoProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.Tree);
                    break;

                case 2:
                    
                    MonsterNameOneProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.Bear);

                    MonsterNameTwoProperty.Value= string.Empty;
                    break;
                case 3:
                    
                    MonsterNameOneProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.ButterFly);

                    MonsterNameTwoProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.Moth);
                    break;
                case 4:
                    
                    MonsterNameOneProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.DragonBird);

                    MonsterNameTwoProperty.Value = GetEnumDescription(FFXIVEnum.MonsterTypeEnum.Ghost);
                    break;

            }

            SelectedMonsterProperty.Value = true;

        }

        public string GetEnumDescription(System.Enum enumValue)
        {
            var str = enumValue.ToString();
            var field = enumValue.GetType().GetField(str);
            var obj = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (obj.Length == 0) return str;
            var da = (System.ComponentModel.DescriptionAttribute)obj[0];
            return da.Description;
        }

        public void ResetFloor()
        {
            TreasureMapProperty.Value.FakeDoor = CorrectDoorTextProperty.Value;

            TreasureMapProperty.Value.Time=DateTime.Now;

            if (CorrectDoorTextProperty.Value == _leftDoor)
                TreasureMapProperty.Value.RealDoor = _rightDoor;
            else if (CorrectDoorTextProperty.Value == _rightDoor)
                TreasureMapProperty.Value.RealDoor = _leftDoor;

            DeepCopy(TreasureMapProperty.Value);


            TreasureMapProperty.Value = TreasureMapProperty.Value.IsInvert ? new TreasureMapModel {IsInvert = true} : new TreasureMapModel( );

            SetMonster(TreasureMapProperty.Value.NowFloor);

            NextFloorProperty.Value = TreasureMapProperty.Value.NowFloor + 1;

            CheckStatus();

          
        }

        public void NextFloor()
        {
            TreasureMapProperty.Value.FakeDoor = CorrectDoorTextProperty.Value;

            TreasureMapProperty.Value.RealDoor = CorrectDoorTextProperty.Value;

            TreasureMapProperty.Value.Time = DateTime.Now;

            DeepCopy(TreasureMapProperty.Value);

            TreasureMapProperty.Value = TreasureMapProperty.Value.IsInvert ? new TreasureMapModel { IsInvert = true } : new TreasureMapModel();

            if (NextFloorProperty.Value==5)
            {
                TreasureMapProperty.Value.NowFloor = 1;
            }
            else
            {
                TreasureMapProperty.Value.NowFloor = NextFloorProperty.Value;
            }
        
            SetMonster(TreasureMapProperty.Value.NowFloor);

            NextFloorProperty.Value = TreasureMapProperty.Value.NowFloor + 1;
            
            CheckStatus();

          


        }

        public  void  DeepCopy(TreasureMapModel other)
        {
            var str = JsonConvert.SerializeObject(other);

            var newObject = JsonConvert.DeserializeObject<TreasureMapModel>(str);

            TreasureHistoryCollection.Add(newObject);

            var strList = JsonConvert.SerializeObject(TreasureHistoryCollection);

           WriteJsonFile(HistoryPathProperty.Value, strList);
        }

        void WriteJsonFile(string path, string jsonConents)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(jsonConents);
                }
            }
        }

       private void GetJsonFile(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
        
            var newObjectList = JsonConvert.DeserializeObject<List<TreasureMapModel>>(json);

            if (newObjectList == null) return;

            TreasureHistoryCollection.Clear();

            foreach (var newObject in newObjectList)
            {
                TreasureHistoryCollection.Add(newObject);
            }

        }


        private void OpenHistory()
        {

            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "TBH文件 (*.json)|*.json"
            };

            var result = dlg.ShowDialog();
            
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                HistoryPathProperty.Value = filename;

                GetJsonFile(filename);
            }
        }
    }
}