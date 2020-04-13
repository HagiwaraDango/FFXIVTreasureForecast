using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using TreasureMakerV2.Enum;

namespace TreasureMakerV2.Model
{
    public  class TreasureMapModel: ObservableObject
    {

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime Time
        {
            get => _time;
            set
            {
                _time = value;
           
                RaisePropertyChanged(() => Time);
            }
        }

        private DateTime _time;
        /// <summary>
        /// 当前层数
        /// </summary>
        public int NowFloor
        {
            get => _nowFloor;
            set
            {
                _nowFloor = value;
                RaisePropertyChanged(() => NowFloor); 
            }
        }
        private int _nowFloor=1;

        /// <summary>
        /// 怪物种类
        /// </summary>
        public FFXIVEnum.MonsterTypeEnum MonsterType
        {
            get => _monsterType;
            set
            {
                _monsterType = value;
                RaisePropertyChanged(() => MonsterType);
            }
        }
        private FFXIVEnum.MonsterTypeEnum _monsterType;
        /// <summary>
        /// 是否发光门
        /// </summary>
        public bool IsBloom
        {
            get => _isBloom;
            set
            {
                _isBloom = value;
                RaisePropertyChanged(() => IsBloom);
            }
        }
        private bool _isBloom;
        /// <summary>
        /// 是否刷新曼德拉
        /// </summary>
        public bool HaveMandra
        {
            get => _haveMandra;
            set
            {
                _haveMandra = value;
                RaisePropertyChanged(() => IsBloom);
            }
        }
        private bool _haveMandra;
        /// <summary>
        /// 是否刷新宝库守卫者
        /// </summary>
        public bool HaveDoorKeeper
        {
            get => _haveDoorKeeper;
            set
            {
                _haveDoorKeeper = value;
                RaisePropertyChanged(() => HaveDoorKeeper);
            }
        }
        private bool _haveDoorKeeper;
        /// <summary>
        /// 是否刷新水妖魔术师
        /// </summary>
        public bool HaveWaterWitcher
        {
            get => _haveWaterWitcher;
            set
            {
                _haveWaterWitcher = value;
                RaisePropertyChanged(() => HaveWaterWitcher);
            }
        }
        private bool _haveWaterWitcher;
        /// <summary>
        /// 是否刷新强欲陷阱 
        /// </summary>
        public bool HaveTrap
        {
            get => _haveTrap;
            set
            {
                _haveTrap = value;
                RaisePropertyChanged(() => HaveTrap);
            }
        }
        private bool _haveTrap;

        /// <summary>
        /// 猜测的门
        /// </summary>
        public string FakeDoor
        {
            get => _isRight;
            set
            {
                _isRight = value;
                RaisePropertyChanged(() => FakeDoor);
            }
        }
        private string _isRight;

        /// <summary>
        /// 实际的门
        /// </summary>
        public string RealDoor
        {
            get => _realDoor;
            set
            {
                _realDoor = value;
                RaisePropertyChanged(() => RealDoor);
            }
        }
        private string _realDoor;

        /// <summary>
        /// 是否反转
        /// </summary>
        public bool IsInvert
        {
            get => _isInvert;
            set
            {
                _isInvert = value;
                RaisePropertyChanged(() => IsInvert);
            }
        }
        private bool _isInvert;

    }
}

