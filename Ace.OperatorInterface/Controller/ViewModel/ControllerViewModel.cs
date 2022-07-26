// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Client;
using Ace.OperatorInterface.ViewModel;
using Ace.Server.Adept.Controllers;
using Ace.Server.Adept.Controllers.Memory;
using Ace.Server.Core.Recipes;
using Ace.Services.LogService;
using Ace.Services.NameLookup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Ace.Communication.Services.Link;


namespace Ace.OperatorInterface.Controller.ViewModel
{
    /// <summary>
    /// ControllerViewModel
    /// </summary>
    public class ControllerViewModel : ItemViewModel
    {
        #region Fields

        public ILogService logService;


        public IRecipeManager recipeManager;
        public bool newFlexiBowlRecipeFound;

        private double _flexiBowlBackLight = 0;

        private double _moveFlipAngle = 0;
        private double _moveFlipAcc = 0;
        private double _moveFlipDec = 0;
        private double _moveFlipSpeed = 0;
        private double _moveFlipDelay = 0;
        private double _moveFlipCount = 0;

        private double _moveAngle = 0;
        private double _moveAcc = 0;
        private double _moveDec = 0;
        private double _moveSpeed = 0;

        private double _flipCount = 0;
        private double _flipDelay = 0;

        private double _moveBlowAngle = 0;
        private double _moveBlowAcc = 0;
        private double _moveBlowDec = 0;
        private double _moveBlowSpeed = 0;
        private double _moveBlowTime = 0;

        private double _blowTime = 0;

        private double _moveBlowFlipAngle = 0;
        private double _moveBlowFlipAcc = 0;
        private double _moveBlowFlipDec = 0;
        private double _moveBlowFlipSpeed = 0;
        private double _moveBlowFlipDelay = 0;
        private double _moveBlowFlipCount = 0;
        private double _moveBlowFlipTime = 0;

        private double _shakeCWAngle = 0;
        private double _shakeCCWAngle = 0;
        private double _shakeAcc = 0;
        private double _shakeDec = 0;
        private double _shakeSpeed = 0;
        private double _shakeCount = 0;

        #endregion


        #region Properties


        /// <summary>
        /// DisplayName
        /// </summary>
        public override string DisplayName
        {
            get
            {
              return "<" + base.DisplayName + ">";
            //  return " "+ base.DisplayName + " ";
            }
        }

        /// <summary>
        /// NameLookupService
        /// </summary>
        protected INameLookupService NameLookupService { get; set; }

        /// <summary>
        /// Background thread to update V+ variable changes
        /// </summary>
        private BackgroundCommandMonitor backgroundMonitor;

        /// <summary>
        /// Controller
        /// </summary>
        public IAdeptController Controller
        {
            get
            {
                return this.ObjectHandle as IAdeptController;
            }            
        }

        /// <summary>
        /// Connection Button Text
        /// </summary>
        public string ConnectionButtonText
        {
            get
            {
                string text = "Connect";
                if (Controller.IsAlive)
                {
                    text = "Disconnect";
                }
                return text;
            }
        }

        /// <summary>
        /// FlexiBowl-BacklLight Button Text
        /// </summary>
        public string FlexiBowlBackLightButtonText
        {
            get
            {
                string text = "Light ON";
                if (Controller.IsAlive)
                {
                    if(FlexiBowlBackLight!=0)
                        text = "Light OFF";
                }
                return text;
            }
        }

        /// <summary>
        /// Gets the toggle connection command.
        /// </summary>
        public DelegateCommand ConnectionCommand { get; private set; }

        /// <summary>
        /// ConnectionHelper
        /// </summary>
        public IConnectionHelper ConnectionHelper { get; private set; }

        /// <summary>
        /// The name of the selected text box
        /// </summary>
        public string SelectedPropertyName { get; set; }


        #region FlexiBowl Properties


        /// <summary>
        /// BackLight Setting
        /// </summary>
        public double FlexiBowlBackLight
        {
            get { return _flexiBowlBackLight; }
            set
            {
                if ((_flexiBowlBackLight != value) && (Controller.IsAlive))
                {

                    bool success = false;
                    double old = _flexiBowlBackLight;

                    SetVPlusValue("fbl.backlight", old, value, out success);
                    if (!success)
                        return;


                    UpdateRecipeManager("fbl.backlight", old, ref value, out success);
                    if (!success)
                        return;

                    
                    _flexiBowlBackLight = value;
                    this.OnPropertyChanged(nameof(FlexiBowlBackLight));
                    //this.UpdateDisplay();                   
                }

            }
        }

        /// <summary>
        /// MOVE-FLIP Command Parameters
        /// </summary>
        public double MoveFlipAngle
        {
            get { return _moveFlipAngle; }
            set
            {
                if ((_moveFlipAngle != value) && (Controller.IsAlive))
                {

                    bool success = false;
                    double old = _moveFlipAngle;

                    UpdateRecipeManager("fbl.mf.angle", old, ref value, out success);
                    if (!success)
                        return;
                    

                    SetVPlusValue("fbl.mf.angle", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipAngle = value;
                    this.OnPropertyChanged(nameof(MoveFlipAngle));
                    //this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipAcc
        {
            get { return _moveFlipAcc; }
            set
            {
                if ((_moveFlipAcc != value) && (Controller.IsAlive))
                {

                    bool success = false;
                    double old = _moveFlipAcc;
             
                    UpdateRecipeManager("fbl.mf.acc", old, ref value, out success);
                    if (!success)
                        return;
        
                    SetVPlusValue("fbl.mf.acc", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipAcc = value;
                    this.OnPropertyChanged(nameof(MoveFlipAcc));
                    //this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipDec
        {
            get { return _moveFlipDec; }
            set
            {
                if ((_moveFlipDec != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveFlipDec;

                    UpdateRecipeManager("fbl.mf.dec", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mf.dec", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipDec = value;
                    this.OnPropertyChanged(nameof(MoveFlipDec));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipSpeed
        {
            get { return _moveFlipSpeed; }
            set
            {
                if ((_moveFlipSpeed != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveFlipSpeed;

                    UpdateRecipeManager("fbl.mf.speed", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mf.speed", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipSpeed = value;
                    this.OnPropertyChanged(nameof(MoveFlipSpeed));
                    //this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipDelay
        {
            get { return _moveFlipDelay; }
            set
            {
                if ((_moveFlipDelay != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveFlipDelay;

                    UpdateRecipeManager("fbl.mf.delay", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mf.delay", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipDelay = value;
                    this.OnPropertyChanged(nameof(MoveFlipDelay));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipCount
        {
            get { return _moveFlipCount; }
            set
            {
                if ((_moveFlipCount != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveFlipCount;

                    UpdateRecipeManager("fbl.mf.count", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mf.count", old, value, out success);
                    if (!success)
                        return;

                    _moveFlipCount = value;
                    this.OnPropertyChanged(nameof(MoveFlipCount));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// MOVE Command Parameters
        /// </summary>
        public double MoveAngle
        {
            get { return _moveAngle; }
            set
            {
                if ((value != _moveAngle) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveAngle;

                    UpdateRecipeManager("fbl.m.angle", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.m.angle", old, value, out success);
                    if (!success)
                        return;

                    _moveAngle = value;
                    this.OnPropertyChanged(nameof(MoveAngle));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveAcc
        {
            get { return _moveAcc; }
            set
            {
                if ((_moveAcc != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveAcc;

                    UpdateRecipeManager("fbl.m.acc", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.m.acc", old, value, out success);
                    if (!success)
                        return;

                    _moveAcc = value;
                    this.OnPropertyChanged(nameof(MoveAcc));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveDec
        {
            get { return _moveDec; }
            set
            {
                if ((_moveDec != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveDec;

                    UpdateRecipeManager("fbl.m.dec", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.m.dec", old, value, out success);
                    if (!success)
                        return;

                    _moveDec = value;
                    this.OnPropertyChanged(nameof(MoveDec));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                if ((value != _moveSpeed) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveSpeed;

                    UpdateRecipeManager("fbl.m.speed", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.m.speed", old, value, out success);
                    if (!success)
                        return;

                    _moveSpeed = value;
                    this.OnPropertyChanged(nameof(MoveSpeed));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// FLIP Command Parameters
        /// </summary>
        public double FlipDelay
        {
            get { return _flipDelay; }
            set
            {
                if ((_flipDelay != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _flipDelay;

                    UpdateRecipeManager("fbl.f.delay", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.f.delay", old, value, out success);
                    if (!success)
                        return;

                    _flipDelay = value;
                    this.OnPropertyChanged(nameof(FlipDelay));
                    //this.UpdateDisplay();
                }
            }
        }
        public double FlipCount
        {
            get { return _flipCount; }
            set
            {
                if ((_flipCount != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _flipCount;

                    UpdateRecipeManager("fbl.f.count", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.f.count", old, value, out success);
                    if (!success)
                        return;

                    _flipCount = value;
                    this.OnPropertyChanged(nameof(FlipCount));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// MOVE-BLOW Command Parameters
        /// </summary>
        public double MoveBlowAngle
        {
            get { return _moveBlowAngle; }
            set
            {
                if ((_moveBlowAngle != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowAngle;

                    UpdateRecipeManager("fbl.mb.angle", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mb.angle", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowAngle = value;
                    this.OnPropertyChanged(nameof(MoveBlowAngle));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowAcc
        {
            get { return _moveBlowAcc; }
            set
            {
                if ((_moveBlowAcc != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowAcc;

                    UpdateRecipeManager("fbl.mb.acc", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mb.acc", old, value, out success);
                    if (!success)
                        return;


                    _moveBlowAcc = value;
                    this.OnPropertyChanged(nameof(MoveBlowAcc));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowDec
        {
            get { return _moveBlowDec; }
            set
            {
                if ((_moveBlowDec != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowDec;

                    UpdateRecipeManager("fbl.mb.dec", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mb.dec", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowDec = value;
                    this.OnPropertyChanged(nameof(MoveBlowDec));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowSpeed
        {
            get { return _moveBlowSpeed; }
            set
            {
                if ((_moveBlowSpeed != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowSpeed;

                    UpdateRecipeManager("fbl.mb.speed", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mb.speed", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowSpeed = value;
                    this.OnPropertyChanged(nameof(MoveBlowSpeed));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowTime
        {
            get { return _moveBlowTime; }
            set
            {
                if ((_moveBlowTime != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowTime;

                    UpdateRecipeManager("fbl.mb.time", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mb.time", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowTime = value;
                    this.OnPropertyChanged(nameof(MoveBlowTime));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// BLOW Command Parameter
        /// </summary>
        public double BlowTime
        {
            get { return _blowTime; }
            set
            {
                if ((_blowTime != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _blowTime;

                    UpdateRecipeManager("fbl.b.time", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.b.time", old, value, out success);
                    if (!success)
                        return;

                    _blowTime = value;
                    this.OnPropertyChanged(nameof(BlowTime));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// MOVE-BLOW-FLIP Command Parameters
        /// </summary>
        public double MoveBlowFlipAngle
        {
            get { return _moveBlowFlipAngle; }
            set
            {
                if ((_moveBlowFlipAngle != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipAngle;

                    UpdateRecipeManager("fbl.mbf.angle", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.angle", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipAngle = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipAngle));
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipAcc
        {
            get { return _moveBlowFlipAcc; }
            set
            {
                if ((_moveBlowFlipAcc != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipAcc;

                    UpdateRecipeManager("fbl.mbf.acc", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.acc", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipAcc = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipAcc));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipDec
        {
            get { return _moveBlowFlipDec; }
            set
            {
                if ((_moveBlowFlipDec != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipDec;

                    UpdateRecipeManager("fbl.mbf.dec", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.dec", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipDec = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipDec));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipSpeed
        {
            get { return _moveBlowFlipSpeed; }
            set
            {
                if ((_moveBlowFlipSpeed != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipSpeed;

                    UpdateRecipeManager("fbl.mbf.speed", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.speed", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipSpeed = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipSpeed));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipDelay
        {
            get { return _moveBlowFlipDelay; }
            set
            {
                if ((_moveBlowFlipDelay != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipDelay;

                    UpdateRecipeManager("fbl.mbf.delay", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.delay", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipDelay = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipDelay));
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipCount
        {
            get { return _moveBlowFlipCount; }
            set
            {
                if ((_moveBlowFlipCount != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipCount;

                    UpdateRecipeManager("fbl.mbf.count", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.count", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipCount = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipCount));
                    // this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipTime
        {
            get { return _moveBlowFlipTime; }
            set
            {
                if ((_moveBlowFlipTime != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _moveBlowFlipTime;

                    UpdateRecipeManager("fbl.mbf.time", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.mbf.time", old, value, out success);
                    if (!success)
                        return;

                    _moveBlowFlipTime = value;
                    this.OnPropertyChanged(nameof(MoveBlowFlipTime));
                    // this.UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// SHAKE Command Parameters
        /// </summary>
        public double ShakeCWAngle
        {
            get { return _shakeCWAngle; }
            set
            {
                if ((_shakeCWAngle != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeCWAngle;

                    UpdateRecipeManager("fbl.sh.cw", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.sh.cw", old, value, out success);
                    if (!success)
                        return;

                    _shakeCWAngle = value;
                    this.OnPropertyChanged(nameof(ShakeCWAngle));
                    // this.UpdateDisplay();
                }
            }
        }
        public double ShakeCCWAngle
        {
            get { return _shakeCCWAngle; }
            set
            {
                if ((_shakeCCWAngle != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeCCWAngle;

                    UpdateRecipeManager("fbl.sh.ccw", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.sh.ccw", old, value, out success);
                    if (!success)
                        return;

                    _shakeCCWAngle = value;
                    this.OnPropertyChanged(nameof(ShakeCCWAngle));
                    // this.UpdateDisplay();
                }
            }
        }
        public double ShakeAcc
        {
            get { return _shakeAcc; }
            set
            {
                if ((_shakeAcc != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeAcc;

                    UpdateRecipeManager("fbl.sh.acc", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.sh.acc", old, value, out success);
                    if (!success)
                        return;

                    _shakeAcc = value;
                    this.OnPropertyChanged(nameof(ShakeAcc));
                    // this.UpdateDisplay();
                }
            }
        }
        public double ShakeDec
        {
            get { return _shakeDec; }
            set
            {
                if ((_shakeDec != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeDec;

                   UpdateRecipeManager("fbl.sh.dec", old, ref value, out success);
                    if (!success)
                        return;

                   SetVPlusValue("fbl.sh.dec", old, value, out success);
                    if (!success)
                        return;

                    _shakeDec = value;
                    this.OnPropertyChanged(nameof(ShakeDec));
                    // this.UpdateDisplay();
                }
            }
        }
        public double ShakeSpeed
        {
            get { return _shakeSpeed; }
            set
            {
                if ((_shakeSpeed != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeSpeed;

                    UpdateRecipeManager("fbl.sh.speed", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.sh.speed", old, value, out success);
                    if (!success)
                        return;

                    _shakeSpeed = value;
                    this.OnPropertyChanged(nameof(ShakeSpeed));
                    // this.UpdateDisplay();
                }
            }
        }
        public double ShakeCount
        {
            get { return _shakeCount; }
            set
            {
                if ((_shakeCount != value) && (Controller.IsAlive))
                {
                    bool success = false;
                    double old = _shakeCount;

                    UpdateRecipeManager("fbl.sh.count", old, ref value, out success);
                    if (!success)
                        return;

                    SetVPlusValue("fbl.sh.count", old, value, out success);
                    if (!success)
                        return;

                    _shakeCount = value;
                    this.OnPropertyChanged(nameof(ShakeCount));
                    // this.UpdateDisplay();
                }
            }
        }

        #endregion FlexiBowl Command Parameter Properties


        /// <summary>
        /// Flexibowl commands
        /// </summary>
        public DelegateCommand FlexibowlCommand_MoveFlip { get; private set; }
        public DelegateCommand FlexibowlCommand_Move { get; private set; }
        public DelegateCommand FlexibowlCommand_Flip { get; private set; }
        public DelegateCommand FlexibowlCommand_MoveBlow { get; private set; }
        public DelegateCommand FlexibowlCommand_Blow { get; private set; }
        public DelegateCommand FlexibowlCommand_MoveBlowFlip { get; private set; }
        public DelegateCommand FlexibowlCommand_Shake { get; private set; }
        public DelegateCommand FlexibowlCommand_Light { get; private set; }
        public DelegateCommand FlexibowlCommand_Empty { get; private set; }
        public DelegateCommand FlexibowlCommand_Reset { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="connectionHelper"></param>
        public ControllerViewModel(INameLookupService nameLookupService, IAdeptController controller, IConnectionHelper connectionHelper):base(controller as IAceObject)
        {
            this.NameLookupService = nameLookupService;

            // Assign method to Action Delegate
            LogMethodDelegate = LogToFile;

            // Assign method to Action Delegate
            BackGroundMonitorDelegate = GetAllVPlusValues;

            // Instantiate background monitor, handle over deleagte 
            this.backgroundMonitor = new BackgroundCommandMonitor(BackGroundMonitorDelegate);

            this.ConnectionHelper = connectionHelper;
            this.ConnectionCommand = new DelegateCommand(ConnectionCommandExecute);

            this.FlexibowlCommand_MoveFlip = new DelegateCommand(FlexibowlExecute_MoveFlip, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Move = new DelegateCommand(FlexibowlExecute_Move, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Flip = new DelegateCommand(FlexibowlExecute_Flip, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_MoveBlow = new DelegateCommand(FlexibowlExecute_MoveBlow, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Blow = new DelegateCommand(FlexibowlExecute_Blow, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_MoveBlowFlip = new DelegateCommand(FlexibowlExecute_MoveBlowFlip, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Shake = new DelegateCommand(FlexibowlExecute_Shake, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Light = new DelegateCommand(FlexibowlExecute_Light, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Empty = new DelegateCommand(FlexibowlExecute_Empty, CanFlexibowlCommandExecute);
            this.FlexibowlCommand_Reset = new DelegateCommand(FlexibowlExecute_Reset, CanFlexibowlCommandExecute);

            LogToFile("Start Background Monitor "+controller.Name);
            backgroundMonitor.Start();

            this.UpdateDisplay();
         
        }

        /// <summary>
        /// Track the property changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnObjectPropertyModified(string propertyName)
        {
            base.OnObjectPropertyModified(propertyName);
            if(propertyName == "IndicatorStateChange" || propertyName == "HighPower")
            {
                this.UpdateDisplay();
            }
        }

        /// <summary>
        /// Implementation of DelegateCommand "ConnectionCommand"
        /// </summary>
        private void ConnectionCommandExecute()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    if (!this.Controller.IsAlive)
                    {
                        LogToFile("ConnectionCommandExecute Connect");
                        this.ConnectionHelper.ConnectToController(this.Controller);

                        // Wait until the connection is established
                        do
                        {
                            System.Threading.Thread.Sleep(500);
                            LogToFile("Wait Connection");
                        }
                        while (!this.Controller.IsAlive);

                        // Get the RecipeManger reference
                        recipeManager = GetRecipeManagerReference();

                        // Activate recipe to store FlexiBowl data in
                        SelectRecipe(recipeManager);

                        LogToFile("Controller " + Controller.Name + " Connected");

                    }
                    else
                    {
                        LogToFile("ConnectionCommandExecute Disconnect");
                        this.ConnectionHelper.DisconnectFromController(this.Controller);

                        this.backgroundMonitor.Stop();  

                        // Zero all FlexiBowl property values to their default for the display of a disconnected controller
                        this.ZeroAllVPlusValues(); 

                    }
                    this.UpdateDisplay();
                });
            }
            catch (Exception ex)
            {
                LogToFile("ConnectionCommandExecute "+ ex.ToString());
                OnReportError(ex.ToString());
            }
        }


        #region Flexibowl DelegateCommands - method implementations

        /// <summary>
        /// Check condition for Execute command being valid.
        /// </summary>
        /// <returns></returns>
        private bool CanFlexibowlCommandExecute()
        {
           //   TODO-s:
           //   Check for modern inline implementation as anonyomous method
           //   Check for V+ variable, indicating "No Q-Seqment running in AppliedMotion Controller (Flexibowl drive)"
                return Controller.IsAlive;
        }

        private void FlexibowlExecute_MoveFlip()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("MoveFlip Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Move()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Move Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Flip()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Flip Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_MoveBlow()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("MoveBlow Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Blow()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Blow Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_MoveBlowFlip()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("MoveBlowFlip Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Shake()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Shake Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Light()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Light Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Empty()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Empty Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }

        private void FlexibowlExecute_Reset()
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    LogToFile("Reset Command");
                });
            }
            catch (Exception ex)
            {
                OnReportError(ex);
            }
            this.UpdateDisplay();
        }
        #endregion


        /// <summary>
        /// Refresh the complete view
        /// </summary>
        private void UpdateDisplay()
        {
            var dispatcher = System.Windows.Application.Current.Dispatcher;
            try {
                dispatcher?.Invoke(() => {
                    this.OnPropertyChanged("ConnectionButtonText");
                    this.ConnectionCommand.RaiseCanExecuteChanged();

                    this.FlexibowlCommand_MoveFlip.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Move.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Flip.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_MoveBlow.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Blow.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_MoveBlowFlip.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Shake.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Light.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Empty.RaiseCanExecuteChanged();
                    this.FlexibowlCommand_Reset.RaiseCanExecuteChanged();

                    this.OnPropertyChanged("FlexiBowlBackLight");
                    this.OnPropertyChanged("MoveFlipAngle");
                    this.OnPropertyChanged("MoveFlipAcc");
                    this.OnPropertyChanged("MoveFlipDec");
                    this.OnPropertyChanged("MoveFlipSpeed");
                    this.OnPropertyChanged("MoveFlipDelay");
                    this.OnPropertyChanged("MoveFlipCount");
                    this.OnPropertyChanged("MoveAngle");
                    this.OnPropertyChanged("MoveAcc");
                    this.OnPropertyChanged("MoveDec");
                    this.OnPropertyChanged("MoveSpeed");
                    this.OnPropertyChanged("FlipDelay");
                    this.OnPropertyChanged("FlipCount");
                    this.OnPropertyChanged("MoveBlowAngle");
                    this.OnPropertyChanged("MoveBlowAcc");
                    this.OnPropertyChanged("MoveBlowDec");
                    this.OnPropertyChanged("MoveBlowSpeed");
                    this.OnPropertyChanged("MoveBlowTime");
                    this.OnPropertyChanged("BlowTime");
                    this.OnPropertyChanged("MoveBlowFlipAngle");
                    this.OnPropertyChanged("MoveBlowFlipAcc");
                    this.OnPropertyChanged("MoveBlowFlipDec");
                    this.OnPropertyChanged("MoveBlowFlipSpeed");
                    this.OnPropertyChanged("MoveBlowFlipDelay");
                    this.OnPropertyChanged("MoveBlowFlipCount");
                    this.OnPropertyChanged("MoveBlowFlipTime");
                    this.OnPropertyChanged("ShakeCWAngle");
                    this.OnPropertyChanged("ShakeCCWAngle");
                    this.OnPropertyChanged("ShakeAcc");
                    this.OnPropertyChanged("ShakeDec");
                    this.OnPropertyChanged("ShakeSpeed");

                    return;
                });

			} catch {
			}
        }

        /// <summary>
        /// Set value of individual V+ Double in Controller Memory
        /// </summary>
        /// <param name="vPlusVariableName"></param>
        /// <param name="previous"></param>
        /// <param name="value"></param>
        /// <param name="success"></param>
        private void SetVPlusValue(string vPlusVariableName,double previous, double value, out bool success)
        {
            try
            {
                LogToFile("V+ Memory Update:     " + vPlusVariableName + " from " + previous.ToString() + " to "+ value.ToString());

                var link = this.Controller.Link;
                link.SetR(vPlusVariableName, value);
                success = true;
            }
            catch (Exception ex)
            {
                LogToFile("SetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
                OnReportError("** SetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString()+" **");
                success = false;
            }
            return;
        }

        /// <summary>
        /// Get value of indivudual V+ Double from Controller Memory. Skip, if this is the selected TextBox.
        /// </summary>
        /// <param name="vPlusVariableName"></param>
        private void GetVPlusValue(string vPlusVariableName, string propertyName, Action<double> setter)
        {
            if (propertyName == SelectedPropertyName) {
                return;
			}

            try
            {

                var link = Controller?.Link;
                if (link != null) {
                    var isDefined = link.ListR(string.Format("DEFINED({0})", vPlusVariableName));
                    if (isDefined != 0) {
                        double value = Controller.GetRealValue(vPlusVariableName);
                        setter(value);
                    }
                }

                //LogToFile("GetVPlusValue " + vPlusVariableName + " = " +  value.ToString());
            }
            catch (Exception ex)
            {
                LogToFile("GetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
                OnReportError("** GetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString() + "**");
            }
        }

        /// <summary>
        /// Read complete list of V+ Doubles in Custom UI. 
        /// Implementation of the Action delegate, getting passed over to the BackgroundCommandMonitor.
        /// </summary>
        private void GetAllVPlusValues()
        {
            if (Controller.IsAlive)
            {
                // LogToFile("Background Task ... GetAllVPlusValues ... ");
                GetVPlusValue("fbl.mf.angle", nameof(MoveFlipAngle), (a) => MoveFlipAngle = a);
                GetVPlusValue("fbl.mf.acc", nameof(MoveFlipAcc), (a) => MoveFlipAcc = a);
                GetVPlusValue("fbl.mf.dec", nameof(MoveFlipDec), (a) => MoveFlipDec = a);
                GetVPlusValue("fbl.mf.speed", nameof(MoveFlipSpeed), (a) => MoveFlipSpeed = a);
                GetVPlusValue("fbl.mf.delay", nameof(MoveFlipDelay), (a) => MoveFlipDelay = a);
                GetVPlusValue("fbl.mf.count", nameof(MoveFlipCount), (a) => MoveFlipCount = a);
                GetVPlusValue("fbl.m.angle", nameof(MoveAngle), (a) => MoveAngle = a);
                GetVPlusValue("fbl.m.acc", nameof(MoveAcc), (a) => MoveAcc = a);
                GetVPlusValue("fbl.m.dec", nameof(MoveDec), (a) => MoveDec = a);
                GetVPlusValue("fbl.m.speed", nameof(MoveSpeed), (a) => MoveSpeed = a);
                GetVPlusValue("fbl.f.delay", nameof(FlipDelay), (a) => FlipDelay = a);
                GetVPlusValue("fbl.f.count", nameof(FlipCount), (a) => FlipCount = a);
                GetVPlusValue("fbl.mb.angle", nameof(MoveBlowAngle), (a) => MoveBlowAngle = a);
                GetVPlusValue("fbl.mb.acc", nameof(MoveBlowAcc), (a) => MoveBlowAcc = a);
                GetVPlusValue("fbl.mb.dec", nameof(MoveBlowDec), (a) => MoveBlowDec = a);
                GetVPlusValue("fbl.mb.speed", nameof(MoveBlowSpeed), (a) => MoveBlowSpeed = a);
                GetVPlusValue("fbl.mb.time", nameof(MoveBlowTime), (a) => MoveBlowTime = a);
                GetVPlusValue("fbl.b.time", nameof(BlowTime), (a) => BlowTime = a);
                GetVPlusValue("fbl.mbf.angle", nameof(MoveBlowFlipAngle), (a) => MoveBlowFlipAngle = a);
                GetVPlusValue("fbl.mbf.acc", nameof(MoveBlowFlipAcc), (a) => MoveBlowFlipAcc = a);
                GetVPlusValue("fbl.mbf.dec", nameof(MoveBlowFlipDec), (a) => MoveBlowFlipDec = a);
                GetVPlusValue("fbl.mbf.speed", nameof(MoveBlowFlipSpeed), (a) => MoveBlowFlipSpeed = a);
                GetVPlusValue("fbl.mbf.delay", nameof(MoveBlowFlipDelay), (a) => MoveBlowFlipDelay = a);
                GetVPlusValue("fbl.mbf.count", nameof(MoveBlowFlipCount), (a) => MoveBlowFlipCount = a);
                GetVPlusValue("fbl.mbf.time", nameof(MoveBlowFlipTime), (a) => MoveBlowFlipTime = a);
                GetVPlusValue("fbl.sh.cw", nameof(ShakeCWAngle), (a) => ShakeCWAngle = a);
                GetVPlusValue("fbl.sh.ccw", nameof(ShakeCCWAngle), (a) => ShakeCCWAngle = a);
                GetVPlusValue("fbl.sh.acc", nameof(ShakeAcc), (a) => ShakeAcc = a);
                GetVPlusValue("fbl.sh.dec", nameof(ShakeDec), (a) => ShakeDec = a);
                GetVPlusValue("fbl.sh.speed", nameof(ShakeSpeed), (a) => ShakeSpeed = a);
                GetVPlusValue("fbl.sh.count", nameof(ShakeCount), (a) => ShakeCount = a);
            }
            else
            {
                // LogToFile("Background Task ... sleep ");
                System.Threading.Thread.Sleep(300);
            }

            return;
        }

        /// <summary>
        /// Zero all fields releated to V+ variables
        /// </summary>
        private void ZeroAllVPlusValues()
        {

            _flexiBowlBackLight = 0;

            _moveFlipAngle = 0;
            _moveFlipAcc = 0;
            _moveFlipDec = 0;
            _moveFlipSpeed = 0;
            _moveFlipDelay = 0;
            _moveFlipCount = 0;

            _moveAngle = 0;
            _moveAcc = 0;
            _moveDec = 0;
            _moveSpeed = 0;

            _flipCount = 0;
            _flipDelay = 0;

            _moveBlowAngle = 0;
            _moveBlowAcc = 0;
            _moveBlowDec = 0;
            _moveBlowSpeed = 0;
            _moveBlowTime = 0;

            _blowTime = 0;

            _moveBlowFlipAngle = 0;
            _moveBlowFlipAcc = 0;
            _moveBlowFlipDec = 0;
            _moveBlowFlipSpeed = 0;
            _moveBlowFlipDelay = 0;
            _moveBlowFlipCount = 0;
            _moveBlowFlipTime = 0;

            _shakeCWAngle = 0;
            _shakeCCWAngle = 0;
            _shakeAcc = 0;
            _shakeDec = 0;
            _shakeSpeed = 0;
            _shakeCount = 0;

            return;
        }

        /// <summary>
        /// Set FlexiBowl default parameter values into the V+ Doubles
        /// </summary>
        private void DefaultAllVPlusValues()
        {
            bool success = false;
            SetVPlusValue("fbl.backlight", _flexiBowlBackLight, 0, out success);
            SetVPlusValue("fbl.mf.angle", _moveFlipAngle, 45, out success);
            SetVPlusValue("fbl.mf.acc", _moveFlipAcc, 250, out success);
            SetVPlusValue("fbl.mf.dec", _moveFlipDec, 250, out success);
            SetVPlusValue("fbl.mf.speed", _moveFlipSpeed, 250, out success);
            SetVPlusValue("fbl.mf.delay", _moveFlipDelay, 50, out success);
            SetVPlusValue("fbl.mf.count", _moveFlipCount, 2, out success);
            SetVPlusValue("fbl.m.angle", _moveAngle, 45, out success);
            SetVPlusValue("fbl.m.acc", _moveAcc, 250, out success);
            SetVPlusValue("fbl.m.dec", _moveDec, 250, out success);
            SetVPlusValue("fbl.m.speed", _moveSpeed,250 , out success);
            SetVPlusValue("fbl.f.delay", _flipCount, 2, out success);
            SetVPlusValue("fbl.f.count", _flipDelay, 50, out success);
            SetVPlusValue("fbl.mb.angle", _moveBlowAngle, 45, out success);
            SetVPlusValue("fbl.mb.acc", _moveBlowAcc, 250, out success);
            SetVPlusValue("fbl.mb.dec", _moveBlowDec, 250, out success);
            SetVPlusValue("fbl.mb.speed", _moveBlowSpeed, 250, out success);
            SetVPlusValue("fbl.mb.time", _moveBlowTime, 200, out success);
            SetVPlusValue("fbl.b.time", _blowTime, 200, out success);
            SetVPlusValue("fbl.mbf.angle", _moveBlowFlipAngle, 45, out success);
            SetVPlusValue("fbl.mbf.acc", _moveBlowFlipAcc, 250, out success);
            SetVPlusValue("fbl.mbf.dec", _moveBlowFlipDec, 250, out success);
            SetVPlusValue("fbl.mbf.speed", _moveBlowFlipSpeed, 250, out success);
            SetVPlusValue("fbl.mbf.delay", _moveBlowFlipDelay, 50, out success);
            SetVPlusValue("fbl.mbf.count", _moveBlowFlipCount, 2, out success);
            SetVPlusValue("fbl.mbf.time", _moveBlowFlipTime, 200, out success);
            SetVPlusValue("fbl.sh.cw", _shakeCWAngle, 45, out success);
            SetVPlusValue("fbl.sh.ccw", _shakeCCWAngle, -45, out success);
            SetVPlusValue("fbl.sh.acc", _shakeAcc, 250, out success);
            SetVPlusValue("fbl.sh.dec", _shakeDec, 250, out success);
            SetVPlusValue("fbl.sh.speed", _shakeSpeed, 250, out success);
            SetVPlusValue("fbl.sh.count", _shakeCount, 1, out success);
            return;
        }

        /// <summary>
        /// Update V+ Variable in currently selected Recipe
        /// </summary>
        /// <param name="vplusVariableName"></param>
        /// <param name="previous"></param>
        /// <param name="value"></param>
        /// <param name="success"></param>
        private void UpdateRecipeManager(string vplusVariableName,double previous, ref double value, out bool success)
        {
            if (recipeManager == null) {
                // Get the RecipeManger reference
                recipeManager = GetRecipeManagerReference();

                // Enforce activate recipe to store FlexiBowl data in
                SelectRecipe(recipeManager);
            };

            if (!Controller.IsAlive)
            {
                LogToFile("** Update Recipe: No Controller Connection **");
                OnReportError("** Cannot update recipe without Controller Connection **");
                success = false;
                return;
            }

            // Correct value to "min <= value <= max" range
            var configuration = recipeManager.Configurations.FirstOrDefault(c => c is IVPlusGlobalVariableCollectionRecipeConfiguration) as IVPlusGlobalVariableCollectionRecipeConfiguration;
            var variableHandle = Controller.Memory.Variables.Variables.FirstOrDefault(v => v.Name == vplusVariableName);
            if (variableHandle != null)
            {
                configuration.GetRealVariableDetails(variableHandle, out double min, out double max, out _, out _);
                if (value < min)
                    value = min;

                if (value > max)
                    value = max;
            }

            // Store this value to active recipe
            var variable = Controller.Memory.Variables.GetVariableByName(vplusVariableName);
            var recipeToken = recipeManager.ActiveRecipe.CreateRecipeReference();
            using (recipeToken)
            {
                var recipe = recipeToken.Recipe;
                var component = recipe.Components.Where(c => c is IVPlusGlobalVariableCollectionRecipeComponent).FirstOrDefault() as IVPlusGlobalVariableCollectionRecipeComponent;

                //LogToFile("Update '"+recipeManager.Name+
                //          "', Recipe '"+recipe.Name+
                //          "', Component'"+component.GetType().Name+
                //          "', Variable '" + variable.Name + "' from " + previous.ToString() + " to " + value.ToString());
                LogToFile(recipe.Name + ": " + variable.Name + "' from " + previous.ToString() + " to " + value.ToString());

                component.SetRealValue(variable, value);
            }
            success = true;
            return;
        }

        /// <summary>
        /// Return a reference to a RecipeManager
        /// </summary>
        /// <returns> recipeManager </returns>
        private IRecipeManager GetRecipeManagerReference()
        {
            if (this.recipeManager == null)
            {
                // Check, if project at least has one RecipeManager
                var recipeManagers = this.NameLookupService[typeof(IRecipeManager)];

                // None - error return
                if (recipeManagers.Length < 1)
                {
                    LogToFile("** ConnectionCommandExecute: Project has no Recipe Manager **");
                    OnReportError("** ConnectionCommandExecute: Project has no Recipe Manager **");
                    return null;
                }

                // Multiple - select the first RecipeManager with "Flexibowl" in the name, or simply the first in the list
                recipeManager = (IRecipeManager)recipeManagers.First();
                if (recipeManagers.Length > 1)
                {
                    foreach (IRecipeManager rm in recipeManagers)
                    {
                        if (rm.Name.Contains("Flexibowl") || rm.Name.Contains("FlexiBowl"))
                        {
                            recipeManager = rm;
                        }
                    }

                    // If no Recipe Manager having Flexibowl in the name, select first
                    LogToFile("** Project has " + recipeManagers.Length.ToString() + " Recipe Managers. "
                               + "Select '" + recipeManager.Name + "'. **");

                    // OnReportError("** Project has " + recipeManagers.Length.ToString() + " Recipe Managers. Select '" + recipeManager.Name + "'. **");
                }
            }
            return this.recipeManager;
        }

        /// <summary>
        /// Enforce having a recipe active on the current RecipeManager
        /// </summary>
        /// <param name="recipeManager"></param>
        private void SelectRecipe(IRecipeManager recipeManager)
        {
            // No recipe selected? Default to 'New FlexiBowl Recipe'.
            if (recipeManager.ActiveRecipe == null)
            {
                LogToFile(recipeManager.Name + " has no Recipe selected'. Will default to 'New FlexiBowl Recipe'");

                // Existing list of recipes? Try scanning for 'New FlexiBowl Recipe' and activate
                if (recipeManager.Recipes.Length > 0)
                {
                    newFlexiBowlRecipeFound = false;
                    foreach (IRecipeToken r in recipeManager.Recipes)
                    {
                        using (IRecipeReference rRecipeReference = r.CreateRecipeReference())
                        {
                            IRecipe rRecipe = rRecipeReference.Recipe;
                            LogToFile(rRecipe.Name);

                            if (rRecipe.Name == "New FlexiBowl Recipe")
                            {// Yes, select 'New FlexiBowl Recipe'
                                newFlexiBowlRecipeFound = true;
                                recipeManager.SelectRecipe(r, true);

                                // Read all FlexiBowl property values from the controller
                                Controller.Memory.Pull();
                                break;

                                // Go ahead and save to 'New FlexiBowl Recipe'
                            }
                        }
                    }
                }

                // 'New FlexiBowl Recipe' NOT found in existing list - create and activate.
                if (!newFlexiBowlRecipeFound)
                {
                    // Write default values to V+ memory
                    this.DefaultAllVPlusValues();

                    // Pull into Variables 
                    Controller.Memory.Pull();

                    // Now, this value snapshot of the variables is getting set in the new recipe

                    // Create and select 'New FlexiBowl Recipe'
                    LogToFile("No 'New FlexiBowl recipe' found in list. Create and select.");

                    if (!CreateAndSelectNewRecipe(recipeManager, "New Flexibowl Recipe"))
                    {// Creating 'New FlexiBowl Recipe' failed
                        LogToFile("FAILED! Create and select 'New FlexiBowl recipe'");
                        OnReportError("FAILED! Create and select 'New FlexiBowl recipe'");
                        return;
                    };

                    // Creating and selecting 'New FlexiBowl Recipe' succeeded
                    // Go ahead and save to 'New FlexiBowl Recipe'
                }
                return;
            }
        }

        /// <summary>
        /// Create new recipe "Recipe 1", rename it to "New FlexiBowl Recipe" and select as active.
        /// </summary>
        /// <param name="recipeManager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CreateAndSelectNewRecipe(IRecipeManager recipeManager, string name)
        {
            try
            {
                recipeManager.AddRecipe();

                foreach (IRecipeToken r in recipeManager.Recipes)
                {
                    using (IRecipeReference rRecipeReference = r.CreateRecipeReference())
                    {
                        IRecipe rRecipe = rRecipeReference.Recipe;

                        if (rRecipe.Name == "Recipe 1")
                        {
                            rRecipe.Name = "New FlexiBowl Recipe";
                            recipeManager.SelectRecipe(r, true);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogToFile("Create new recipe exception: " + ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Type-Debugging into text-file. Implementation of method, assigned to LogMethodDelegate Action-Delegate.
        /// </summary>
        /// <param name="text"></param>
        static void LogToFile(string text)
        {
            System.IO.File.AppendAllText("C:\\temp\\log.txt", text+Environment.NewLine);
        }

        public override void UnhookEvents() {
            base.UnhookEvents();
            backgroundMonitor.Stop();
        }
    }
}