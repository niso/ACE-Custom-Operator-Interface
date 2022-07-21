// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.ViewModel;
using Ace.Server.Adept.Controllers;
using Ace.Server.Adept.Controllers.Memory;
using Ace.Server.Core.Recipes;
using Ace.Services.NameLookup;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Ace.OperatorInterface.Controller.ViewModel
{
    /// <summary>
    /// ControllerViewModel
    /// </summary>
    public class ControllerViewModel : ItemViewModel
    {
        #region Fields

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
            //  return "<" + base.DisplayName + ">";
                return " "+ base.DisplayName + " ";
            }
        }

        /// <summary>
        /// NameLookupService
        /// </summary>
        protected INameLookupService NameLookupService { get; set; }

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
        /// BackLight Setting
        /// </summary>
        public double FlexiBowlBackLight
        {
            get { return _flexiBowlBackLight; }
            set
            {
                if (_flexiBowlBackLight != value)
                {

                    bool success = false;
                    double old = _flexiBowlBackLight;

                    SetVPlusValue("fbl.backlight", old, value, out success);
                    if (!success)
                        return;

                    UpdateRecipeManager("fbl.backlight", old, value, out success);
                    if (success)
                    {
                        _flexiBowlBackLight = value;
                        this.OnPropertyChanged(nameof(FlexiBowlBackLight));
                    }
                }
                this.UpdateDisplay();
            }
        }


        #region FlexiBowl Command Parameter Properties

        /// <summary>
        /// MOVE-FLIP Command Parameters
        /// </summary>
        public double MoveFlipAngle {
            get { return _moveFlipAngle; }
            set {
                if (_moveFlipAngle != value) {

                    bool success = false;
                    double old = _moveFlipAngle;

                    UpdateRecipeManager("fbl.mf.angle",old, value, out success);
                    if (success)
                    {
                        _moveFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveFlipAngle));
                    }

                    SetVPlusValue("fbl.mf.angle",old, value, out success);
                    if (!success)
                        return;
                }
                this.UpdateDisplay();
            }
        }
        public double MoveFlipAcc {
            get { return _moveFlipAcc; }
            set
            {
                if (_moveFlipAcc != value) {

                    bool success = false;
                    double old = _moveFlipAcc;

                    UpdateRecipeManager("fbl.mf.acc",old, value, out success);
                    SetVPlusValue("fbl.mf.acc",old, value, out success);

                    if (success)
                    {
                        _moveFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveFlipAcc));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipDec {
            get { return _moveFlipDec; }
            set
            {
                if (_moveFlipDec != value)
                {
                    bool success = false;
                    double old = _moveFlipDec;

                    SetVPlusValue("fbl.mf.dec", old, value,out success);
                    UpdateRecipeManager("fbl.mf.dec", old, value, out success);

                    if (success)
                    {
                        _moveFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveFlipDec));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipSpeed {
            get { return _moveFlipSpeed; }
            set
            {
                if( _moveFlipSpeed != value)
                {
                    bool success = false;
                    double old= _moveFlipSpeed;

                    SetVPlusValue("fbl.mf.speed",old, value, out success);
                    UpdateRecipeManager("fbl.mf.speed",old, value, out success);

                    if (success)
                    {
                        _moveFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveFlipSpeed));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipDelay
        {
            get { return _moveFlipDelay; }
            set
            {
                if(_moveFlipDelay != value)
                {
                    bool success = false;
                    double old = _moveFlipDelay;

                    SetVPlusValue("fbl.mf.delay",old, value, out success);
                    UpdateRecipeManager("fbl.mf.delay",old, value, out success);

                    if (success)
                    {
                        _moveFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveFlipDelay));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveFlipCount
        {
            get { return _moveFlipCount; }
            set
            {
                if(_moveFlipCount != value)
                {
                    bool success = false;
                    double old = _moveFlipCount;

                    SetVPlusValue("fbl.mf.count",old, value, out success);
                    UpdateRecipeManager("fbl.mf.count",old, value, out success);

                    if (success)
                    {
                        _moveFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveFlipCount));
                    }
                    this.UpdateDisplay();
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
                if(value != _moveAngle)
                {
                    bool success = false;
                    double old = _moveAngle;

                    SetVPlusValue("fbl.m.angle",old, value, out success);
                    UpdateRecipeManager("fbl.m.angle",old, value, out success);

                    if (success)
                    {
                        _moveAngle = value;
                        this.OnPropertyChanged(nameof(MoveAngle));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveAcc
        {
            get { return _moveAcc; }
            set
            {
                if(_moveAcc != value)
                {
                    bool success = false;
                    double old = _moveAcc;  

                    SetVPlusValue("fbl.m.acc",old, value,out success);
                    UpdateRecipeManager("fbl.m.acc",old, value, out success);

                    if (success)
                    {
                        _moveAcc = value;
                        this.OnPropertyChanged(nameof(MoveAcc));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveDec
        {
            get { return _moveDec; }
            set
            {
                if(_moveDec != value)
                {
                    bool success = false;
                    double old = _moveDec;

                    SetVPlusValue("fbl.m.dec", old,value,out success);
                    UpdateRecipeManager("fbl.m.dec",old, value, out success);

                    if (success)
                    {
                        _moveDec = value;
                        this.OnPropertyChanged(nameof(MoveDec));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                if(value != _moveSpeed)
                {
                    bool success = false;
                    double old = _moveSpeed;

                    SetVPlusValue("fbl.m.speed",old, value,out success);
                    UpdateRecipeManager("fbl.m.speed",old, value, out success);

                    if(success)
                    {
                    _moveSpeed = value;
                    this.OnPropertyChanged(nameof(MoveSpeed));
                    }
                    this.UpdateDisplay();
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
                if(_flipDelay != value)
                {
                    bool success = false;
                    double old = _flipDelay;

                    SetVPlusValue("fbl.f.delay",old, value, out success);
                    UpdateRecipeManager("fbl.f.delay",old, value, out success);

                    if (success)
                    {
                        _flipDelay = value;
                        this.OnPropertyChanged(nameof(FlipDelay));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double FlipCount {
            get { return _flipCount; }
            set
            {
                if(_flipCount != value)
                {
                    bool success = false;
                    double old = _flipCount;

                    SetVPlusValue("fbl.f.count",old, value,out success);
                    UpdateRecipeManager("fbl.f.count",old, value, out success);

                    if (success)
                    {
                        _flipCount = value;
                        this.OnPropertyChanged(nameof(FlipCount));
                    }
                    this.UpdateDisplay();
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
                if(value != _moveBlowAngle)
                {
                    bool success = false;
                    double old = _moveBlowAngle;

                    SetVPlusValue("fbl.mb.angle",old, value, out success);
                    UpdateRecipeManager("fbl.mb.angle",old, value, out success);

                    if (success)
                    {
                        _moveBlowAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowAngle));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowAcc
        {
            get { return _moveBlowAcc; }
            set
            {
                if(_moveBlowAcc != value)
                {
                    bool success = false;
                    double old = _moveBlowAcc;

                    SetVPlusValue("fbl.mb.acc",old, value,out success);
                    UpdateRecipeManager("fbl.mb.acc",old, value, out success);

                    if (success)
                    {
                        _moveBlowAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowAcc));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowDec
        {
            get { return _moveBlowDec; }
            set
            {
                if(_moveBlowDec != value)
                {
                    bool success = false;
                    double old = _moveBlowDec;

                    SetVPlusValue("fbl.mb.dec",old, value,out success);
                    UpdateRecipeManager("fbl.mb.dec",old, value, out success);

                    if (success)
                    {
                        _moveBlowDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowDec));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowSpeed
        {
            get { return _moveBlowSpeed; }
            set
            {
                if(_moveBlowSpeed != value)
                {
                    bool success = false;
                    double old = _moveBlowSpeed;

                    SetVPlusValue("fbl.mb.speed",old, value,out success);
                    UpdateRecipeManager("fbl.mb.speed",old, value, out success);

                    if (success)
                    {
                        _moveBlowSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowSpeed));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowTime
        {
            get { return _moveBlowTime; }
            set
            {
                if(value != _moveBlowTime)
                {
                    bool success = false;
                    double old = _moveBlowTime;

                    SetVPlusValue("fbl.mb.time",old, value, out success);
                    UpdateRecipeManager("fbl.mb.time",old, value, out success);

                    if (success)
                    {
                        _moveBlowTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowTime));
                    }
                    this.UpdateDisplay();
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
                if(_blowTime != value)
                {
                    bool success = false;
                    double old = _blowTime;

                    SetVPlusValue("fbl.b.time",old, value, out success);
                    UpdateRecipeManager("fbl.b.time",old, value, out success);

                    if (success)
                    {
                        _blowTime = value;
                        this.OnPropertyChanged(nameof(BlowTime));
                    }
                    this.UpdateDisplay();
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
                if(_moveBlowFlipAngle != value)
                {
                    bool success = false;
                    double old = _moveBlowFlipAngle;

                    SetVPlusValue("fbl.mbf.angle",old, value, out success);
                    UpdateRecipeManager("fbl.mbf.angle",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAngle));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipAcc
        {
            get { return _moveBlowFlipAcc; }
            set
            {
                if(_moveBlowFlipAcc != value)
                {
                    bool success = false;
                    double old = _moveBlowFlipAcc;

                    SetVPlusValue("fbl.mbf.acc", old, value, out success);
                    UpdateRecipeManager("fbl.mbf.acc",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAcc));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipDec
        {
            get { return _moveBlowFlipDec; }
            set
            {
                if(value != _moveBlowFlipDec)
                {
                    bool success = false;
                    double old = _moveBlowFlipDec;

                    SetVPlusValue("fbl.mbf.dec", old, value, out success);
                    UpdateRecipeManager("fbl.mbf.dec",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDec));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipSpeed
        {
            get { return _moveBlowFlipSpeed; }
            set
            {
                if(value!= _moveBlowFlipSpeed)
                {
                    bool success = false;
                    double old = _moveBlowFlipSpeed;

                    SetVPlusValue("fbl.mbf.speed", old, value, out success);
                    UpdateRecipeManager("fbl.mbf.speed",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipSpeed));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipDelay
        {
            get { return _moveBlowFlipDelay; }
            set
            {
                if(value != _moveBlowFlipDelay)
                {
                    bool success = false;
                    double old = _moveBlowFlipDelay;

                    SetVPlusValue("fbl.mbf.delay", old, value, out success);
                    UpdateRecipeManager("fbl.mbf.delay",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDelay));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipCount
        {
            get { return _moveBlowFlipCount; }
            set
            {
                if(_moveBlowFlipCount != value)
                {
                    bool success = false;
                    double old = _moveBlowFlipCount;

                    SetVPlusValue("fbl.mbf.count", old, value,out success);
                    UpdateRecipeManager("fbl.mbf.count",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipCount));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double MoveBlowFlipTime
        {
            get { return _moveBlowFlipTime; }
            set
            {
                if(value!= _moveBlowFlipTime)
                {
                    bool success = false;
                    double old = _moveBlowFlipTime;

                    SetVPlusValue("fbl.mbf.time",old, value, out success);
                    UpdateRecipeManager("fbl.mbf.time",old, value, out success);

                    if (success)
                    {
                        _moveBlowFlipTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipTime));
                    }
                    this.UpdateDisplay();
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
                if(value != _shakeCWAngle)
                {
                    bool success = false;
                    double old = _shakeCWAngle;

                    SetVPlusValue("fbl.sh.cw", old, value, out success);
                    UpdateRecipeManager("fbl.sh.cw",old, value, out success);

                    if (success)
                    {
                        _shakeCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCWAngle));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double ShakeCCWAngle
        {
            get { return _shakeCCWAngle; }
            set
            {
                if( value != _shakeCCWAngle)
                {
                    bool success = false;
                    double old = _shakeCCWAngle;

                    SetVPlusValue("fbl.sh.ccw", old, value, out success);
                    UpdateRecipeManager("fbl.sh.ccw",old, value, out success);

                    if (success)
                    {
                        _shakeCCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCCWAngle));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double ShakeAcc
        {
            get { return _shakeAcc; }
            set
            {
                if (value != _shakeAcc)
                {
                    bool success = false;
                    double old = _shakeAcc;

                    SetVPlusValue("fbl.sh.acc", old, value, out success);
                    UpdateRecipeManager("fbl.sh.acc",old, value, out success);

                    if (success)
                    {
                        _shakeAcc = value;
                        this.OnPropertyChanged(nameof(ShakeAcc));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double ShakeDec
        {
            get { return _shakeDec; }
            set
            {
                if(value != _shakeDec)
                {
                    bool success = false;
                    double old = _shakeDec;

                    SetVPlusValue("fbl.sh.dec",old, value, out success);
                    UpdateRecipeManager("fbl.sh.dec",old, value, out success);

                    if (success)
                    {
                        _shakeDec = value;
                        this.OnPropertyChanged(nameof(ShakeDec));
                    }
                    this.UpdateDisplay();
                }

            }
        }
        public double ShakeSpeed
        {
            get { return _shakeSpeed; }
            set
            {
                if(value!= _shakeSpeed)
                {
                    bool success = false;
                    double old = _shakeSpeed;

                    SetVPlusValue("fbl.sh.speed", old, value, out success);
                    UpdateRecipeManager("fbl.sh.speed",old, value, out success);

                    if (success)
                    {
                        _shakeSpeed = value;
                        this.OnPropertyChanged(nameof(ShakeSpeed));
                    }
                    this.UpdateDisplay();
                }
            }
        }
        public double ShakeCount
        {
            get { return _shakeCount; }
            set
            {
                if(value != _shakeCount)
                {
                    bool success = false;
                    double old = _shakeCount;

                    SetVPlusValue("fbl.sh.count", old, value, out success);
                    UpdateRecipeManager("fbl.sh.count",old, value, out success);

                    if (success)
                    {
                        _shakeCount = value;
                        this.OnPropertyChanged(nameof(ShakeCount));
                    }
                    this.UpdateDisplay();
                }
            }
        }


        #endregion FlexiBowl Command Parameter Properties


        /// <summary>
        /// Flexibowl command, using CommandParameter to differ between them
        /// </summary>
        public DelegateCommand FlexiBowlCommand { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="connectionHelper"></param>
        public ControllerViewModel(INameLookupService nameLookupService, IAdeptController controller, IConnectionHelper connectionHelper):base(controller as IAceObject)
        {
            this.NameLookupService = nameLookupService;

            logMethod = LogToFile;

            this.ConnectionHelper = connectionHelper;
            this.ConnectionCommand = new DelegateCommand(ConnectionCommandExecute);

            this.UpdateDisplay();
         
        }

        /// <summary>
        /// track the property changed
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



                        // Check, if project at least has one RecipeManager
                        var recipeManagers = this.NameLookupService[typeof(IRecipeManager)];
                        if (recipeManagers.Length < 1)
                        {
                            LogToFile("** ConnectionCommandExecute: Project has no Recipe Manager **");
                            OnReportError("** ConnectionCommandExecute: Project has no Recipe Manager **");
                            return;
                        }

                        // If multiple RecipeManagers, select the first RecipeManager with "Flexibowl" in the name, or simply the first in the list
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

                        // No recipe selected? Default to 'New FlexiBowl Recipe'.
                        if (recipeManager.ActiveRecipe == null)
                        {
                            LogToFile("No Recipe selected in Recipe Manager '" + recipeManager.Name + "'. Will default to 'New FlexiBowl Recipe'");

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
                                            this.GetAllVPlusValues();

                                            break;

                                            // Go ahead and save to 'New FlexiBowl Recipe'
                                        }
                                    }
                                }
                            }
                            
                            // 'New FlexiBowl Recipe' NOT found in existing list
                            if (!newFlexiBowlRecipeFound)
                            {
                                // Write default values to V+ memory
                                this.DefaultAllVPlusValues();

                                // Pull into Variables 
                                Controller.Memory.Pull();


                                // Create and select 'New FlexiBowl Recipe'
                                LogToFile("No 'New FlexiBowl recipe' found in list. Create and select.");
                                //  OnReportError("No 'New FlexiBowl recipe' found in list. Create and select.");

                                    if (!CreateAndSelectNewRecipe(recipeManager, "New Flexibowl Recipe"))
                                    {// Creating 'New FlexiBowl Recipe' failed
                                        LogToFile("FAILED! Create and select 'New FlexiBowl recipe'");
                                        OnReportError("FAILED! Create and select 'New FlexiBowl recipe'");
                                        return;
                                    };
                                    // Creating and selecting 'New FlexiBowl Recipe' succeeded
                                    // Go ahead and save to 'New FlexiBowl Recipe'
                                }
                        }
                        LogToFile("Connected");

                    }
                    else
                    {
                        LogToFile("ConnectionCommandExecute Disconnect");
                        this.ConnectionHelper.DisconnectFromController(this.Controller);

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

        private void UpdateDisplay()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                this.OnPropertyChanged("ConnectionButtonText");
                this.ConnectionCommand.RaiseCanExecuteChanged();


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
        }

        private void SetVPlusValue(string vPlusVariableName,double previous, double value, out bool success)
        {

            // This gets called 
            try
            {
                LogToFile("SetVPlusValue " + vPlusVariableName + " from " + previous.ToString() + " to "+ value.ToString());

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

        private double GetVPlusValue(string vPlusVariableName)
        {
            double value;
            try
            {
                //var link = this.Controller.Link;
                //value = link.ListR(vPlusVariableName);
                value = Controller.GetRealValue(vPlusVariableName);


                LogToFile("GetVPlusValue " + vPlusVariableName + " = " +  value.ToString());
            }
            catch (Exception ex)
            {
                LogToFile("GetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
                OnReportError("** GetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString() + "**");
                value = 0;
            }
            return value;
        }

        private void GetAllVPlusValues()
        {
            LogToFile("GetAllVPlusValues ... ");
            _moveFlipAngle = GetVPlusValue("fbl.mf.angle");
            _moveFlipAcc = GetVPlusValue("fbl.mf.acc");
            _moveFlipDec = GetVPlusValue("fbl.mf.dec");
            _moveFlipSpeed = GetVPlusValue("fbl.mf.speed");
            _moveFlipDelay = GetVPlusValue("fbl.mf.delay");
            _moveFlipCount = GetVPlusValue("fbl.mf.count");
            _moveAngle = GetVPlusValue("fbl.m.angle");
            _moveAcc = GetVPlusValue("fbl.m.acc");
            _moveDec = GetVPlusValue("fbl.m.dec");
            _moveSpeed = GetVPlusValue("fbl.m.speed");
            _flipDelay = GetVPlusValue("fbl.f.delay");
            _flipCount = GetVPlusValue("fbl.f.count");
            _moveBlowAngle = GetVPlusValue("fbl.mb.angle");
            _moveBlowAcc = GetVPlusValue("fbl.mb.acc");
            _moveBlowDec = GetVPlusValue("fbl.mb.dec");
            _moveBlowSpeed = GetVPlusValue("fbl.mb.speed");
            _moveBlowTime = GetVPlusValue("fbl.mb.time");
            _blowTime = GetVPlusValue("fbl.b.time");
            _moveBlowFlipAngle = GetVPlusValue("fbl.mbf.angle");
            _moveBlowFlipAcc = GetVPlusValue("fbl.mbf.acc");
            _moveBlowFlipDec = GetVPlusValue("fbl.mbf.dec");
            _moveBlowFlipSpeed = GetVPlusValue("fbl.mbf.speed");
            _moveBlowFlipDelay = GetVPlusValue("fbl.mbf.delay");
            _moveBlowFlipCount = GetVPlusValue("fbl.mbf.count");
            _moveBlowFlipTime = GetVPlusValue("fbl.mbf.time");
            _shakeCWAngle = GetVPlusValue("fbl.sh.cw");
            _shakeCCWAngle = GetVPlusValue("fbl.sh.ccw");
            _shakeAcc = GetVPlusValue("fbl.sh.acc");
            _shakeDec = GetVPlusValue("fbl.sh.dec");
            _shakeSpeed = GetVPlusValue("fbl.sh.speed");
            _shakeCount = GetVPlusValue("fbl.sh.count");

            return;
        }

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


            //_flexiBowlBackLight = 0;

            //_moveFlipAngle = 45;
            //_moveFlipAcc = 250;
            //_moveFlipDec = 250;
            //_moveFlipSpeed = 250;
            //_moveFlipDelay = 50;
            //_moveFlipCount = 2;

            //_moveAngle = 45;
            //_moveAcc = 250;
            //_moveDec = 250;
            //_moveSpeed = 250;

            //_flipCount = 2;
            //_flipDelay = 50;

            //_moveBlowAngle = 45;
            //_moveBlowAcc = 250;
            //_moveBlowDec = 250;
            //_moveBlowSpeed = 250;
            //_moveBlowTime = 200;

            //_blowTime = 200;

            //_moveBlowFlipAngle = 45;
            //_moveBlowFlipAcc = 250;
            //_moveBlowFlipDec = 250;
            //_moveBlowFlipSpeed = 250;
            //_moveBlowFlipDelay = 50;
            //_moveBlowFlipCount = 2;
            //_moveBlowFlipTime = 200;

            //_shakeCWAngle = 45;
            //_shakeCCWAngle = -45;
            //_shakeAcc = 250;
            //_shakeDec = 250;
            //_shakeSpeed = 250;
            //_shakeCount = 1;

            return;
        }

        private void UpdateRecipeManager(string vplusVariableName,double previous, double value, out bool success)
        {
            // Check, if Controller connected
            if (!Controller.IsAlive)
            {
                LogToFile("** Update Recipe: No Controller Connection **");
                OnReportError("** Cannot update recipe without Controller Connection **");
                success = false;
                return;
            }

            //// Check, if project at least has one RecipeManager
            //var recipeManagers = this.NameLookupService[typeof(IRecipeManager)];
            //if (recipeManagers.Length < 1)
            //{
            //    LogToFile("** Update Recipe: Project has no Recipe Manager **");
            //    OnReportError("** Update Recipe: Project has no Recipe Manager **");
            //    success = false;
            //    return;
            //}

            //// If multiple RecipeManagers, select the first with "Flexibowl" in the name,
            //// or simply the first in the list
            //var recipeManager = (IRecipeManager)recipeManagers.First(); ;
            //if (recipeManagers.Length > 1)
            //{
            //    foreach(IRecipeManager rm in recipeManagers)
            //    {
            //        if (rm.Name.Contains("Flexibowl") || rm.Name.Contains("FlexiBowl"))
            //        {
            //            recipeManager = rm;
            //        }
            //    }

            //    // If no Recipe Manager having Flexibowl in the name, select first
            //    LogToFile("** Project has "+recipeManagers.Length.ToString()+" Recipe Managers. " 
            //               + "Select '"+recipeManager.Name+"'. **");

            //    OnReportError("** Project has " + recipeManagers.Length.ToString() + " Recipe Managers. " 
            //                   + "Select '"+recipeManager.Name+ "'. **");
            //}

            //// Is any recipe selected to store data in? If not, default to 'New FlexiBowl Recipe'.
            //if (recipeManager.ActiveRecipe == null)
            //{// No recipe is selected

            //    LogToFile("No Recipe selected in Recipe Manager '" + recipeManager.Name+"'. Will default to 'New FlexiBowl Recipe'");

            //    // Is there a list of available recipes?
            //    if (recipeManager.Recipes.Length > 0)
            //    {// Yes, there is a list of available recipes

            //        // Select recipe 'New FlexiBowl Recipe' if present
            //        bool newFlexiBowlRecipeFound = false;
            //        foreach (IRecipeToken r in recipeManager.Recipes)
            //        {
            //            using (IRecipeReference rRecipeReference = r.CreateRecipeReference())
            //            {
            //                IRecipe rRecipe = rRecipeReference.Recipe;
            //                LogToFile(rRecipe.Name);

            //                if (rRecipe.Name == "New FlexiBowl Recipe")
            //                {// Yes, select 'New FlexiBowl Recipe'
            //                    newFlexiBowlRecipeFound = true;
            //                    recipeManager.SelectRecipe(r, true);
            //                    break;

            //                    // Go ahead and save to 'New FlexiBowl Recipe'
            //                }
            //            }
            //        }

            //        // 'New FlexiBowl Recipe' NOT found in existing list
            //        if (!newFlexiBowlRecipeFound)
            //        {
            //            // Create and select 'New FlexiBowl Recipe'
            //            LogToFile("No 'New FlexiBowl recipe' found in list. Create and select.");
            //            OnReportError("No 'New FlexiBowl recipe' found in list. Create and select.");

            //            if (!CreateAndSelectNewRecipe(recipeManager, "New Flexibowl Recipe"))
            //            {// Creating 'New FlexiBowl Recipe' failed
            //                LogToFile("FAILED! Create and select 'New FlexiBowl recipe'");
            //                OnReportError("FAILED! Create and select 'New FlexiBowl recipe'");
            //                success = false;
            //                return;
            //            };
            //            // Creating and selecting 'New FlexiBowl Recipe' succeeded
            //            // Go ahead and save to 'New FlexiBowl Recipe'
            //        }
            //    }
            //    else
            //    {// No, there is no list of available recipes yet ...

            //        // Create new and select 'New FlexiBowl Recipe'
            //        LogToFile("No available recipes. Create and select 'New FlexiBowl recipe'");
            //        OnReportError("No available recipes. Create and select 'New FlexiBowl recipe'");

            //        if(!CreateAndSelectNewRecipe(recipeManager,"New Flexibowl Recipe"))
            //        {// Creating 'New FlexiBowl Recipe' failed
            //            LogToFile("FAILED! No available recipes. Create and select 'New FlexiBowl recipe'");
            //            OnReportError("FAILED! No available recipes. Create and select 'New FlexiBowl recipe'");
            //            success = false;
            //            return;
            //        };
            //        // Creating and selecting 'New FlexiBowl Recipe' succeeded
            //    }
            //}


            // Store this value to active recipe
            var variable = Controller.Memory.Variables.GetVariableByName(vplusVariableName);
            var recipeToken = recipeManager.ActiveRecipe.CreateRecipeReference();
            using (recipeToken)
            {
                var recipe = recipeToken.Recipe;
                var component = recipe.Components.Where(c => c is IVPlusGlobalVariableCollectionRecipeComponent).FirstOrDefault() as IVPlusGlobalVariableCollectionRecipeComponent;

                LogToFile("Update '"+recipeManager.Name+
                          "', Recipe '"+recipe.Name+
                          "', Component'"+component.GetType().Name+
                          "', Variable '" + variable.Name + "' from " + previous.ToString() + " to " + value.ToString());
                component.SetRealValue(variable, value);
            }
            success = true;
            return;
        }

        private bool CreateAndSelectNewRecipe(IRecipeManager recipeManager, string name)
        {
            // Create new recipe "Recipe 1" and rename it to "New FlexiBowl Recipe".
            // Select "New FlexiBowl Recipe"
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

        static void LogToFile(string text)
        {
            System.IO.File.AppendAllText("C:\\temp\\log.txt", text+Environment.NewLine);
        }
    }
}