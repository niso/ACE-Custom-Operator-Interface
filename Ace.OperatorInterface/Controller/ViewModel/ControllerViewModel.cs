// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.Client;
using Ace.OperatorInterface.ViewModel;
using Ace.Server.Adept.Controllers;
using Ace.Server.Adept.Controllers.Memory;
using Ace.Server.Core.Recipes;
using Ace.Server.Core.Variable;
using Ace.Services.LogService;
using Ace.Services.NameLookup;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Shapes;
using System.ComponentModel;
//using System.Collections.Concurrent;
//using System.IO;
//using System.Threading;

namespace Ace.OperatorInterface.Controller.ViewModel
{
    /// <summary>
    /// ControllerViewModel
    /// </summary>
    public class ControllerViewModel : ItemViewModel
    {

        #region Fields

        public ILogService logService;

        private static object lockObject = new object();

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

        private double _fbParamSet = 15;
        private double _fbMove = 2;
        private double _fbMoveFlip = 3;
        private double _fbFlip = 10;
        private double _fbMoveBlow = 5;
        private double _fbBlow = 9;
        private double _fbMoveBlowFlip = 4;
        private double _fbShake = 6;
        //private double _fbLightOn = 7;
        //private double _fbLightOff = 8;
        //private double _fbQickEmpty = 11;
        //private double _fbResetAlarm = 12;

        /// <summary>
        /// Background thread to update V+ variable and recipe selection changes
        /// </summary>
        private BackgroundCommandMonitor backgroundMonitor;


        public static string aceAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\OMRON\ACE";

        public IRecipeManager recipeManager;
        private string _lastActiveRecipe = "";

        public bool storedRecipeFound;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Last active recipe - persisted to %APPDATA\OMRON\ACE\LastActiveRecipe.txt
        /// (Workaround because ACE by itself does not save the last recipe selected into the workspace project)
        /// </summary>
        public string LastActiveRecipe
        {
            get
            {
                if (File.Exists(aceAppDataFolder + @"\LastActiveRecipe.txt"))
                {
                    // Return value from save file
                    _lastActiveRecipe = File.ReadAllText(aceAppDataFolder + @"\LastActiveRecipe.txt");

                }
                return _lastActiveRecipe;
            }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_lastActiveRecipe != value)
                    {
                        // Delete old save file
                        if (File.Exists(aceAppDataFolder + @"\LastActiveRecipe.txt"))
                        {
                            System.IO.File.Delete(aceAppDataFolder + @"\LastActiveRecipe.txt");
                        }
                        // Save new file
                        System.IO.File.AppendAllText(aceAppDataFolder + @"\LastActiveRecipe.txt", value);
                        this.OnPropertyChanged(nameof(LastActiveRecipe));
                    }
                }
            }
        }

        /// <summary>
        /// DisplayName
        /// </summary>
        public override string DisplayName
        {
            get
            {
              return "<" + base.DisplayName + ">";
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
        /// FlexiBowl-BacklLight Button Text
        /// </summary>
        public string FlexiBowlBackLightButtonText
        {
            get
            {
                string text = "Light ON";
                if (Controller.IsAlive)
                {
                    if (FlexiBowlBackLight != 0)
                    {
                        text = "Light OFF";
                    }
                    else
                    {
                        text = "Light ON";
                    }
                }
                else
                {
                    text = "---";
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


        #region FlexiBowl Parameter Properties

        /// <summary>
        /// BackLight Setting
        /// </summary>
        public double FlexiBowlBackLight
        {
            get { return _flexiBowlBackLight; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_flexiBowlBackLight != value)
                    {
                        if (!RecipeManagerUpdate("fbl.backlight", value)) return;
                        if (!SetVPlusValue("fbl.backlight", value)) return;

                        _flexiBowlBackLight = value;
                        this.OnPropertyChanged(nameof(FlexiBowlBackLight));
                    }
                }
                else
                {
                    if (_flexiBowlBackLight != value)
                    {
                        _flexiBowlBackLight = value;
                        this.OnPropertyChanged(nameof(FlexiBowlBackLight));
                    }
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
                if (Controller.IsAlive)
                {
                    if (_moveFlipAngle != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.angle", value)) return;
                        if (!SetVPlusValue("fbl.mf.angle", value)) return;

                        _moveFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveFlipAngle));
                    }
                }
                else
                {
                    if (_moveFlipAngle != value)
                    {
                        _moveFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveFlipAngle));
                    }
                }
            }
        }

        public double MoveFlipAcc
        {
            get { return _moveFlipAcc; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveFlipAcc != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.acc", value)) return;
                        if (!SetVPlusValue("fbl.mf.acc", value)) return;

                        _moveFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveFlipAcc));
                    }
                }
                else
                {
                    if (_moveFlipAcc != value)
                    {
                        _moveFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveFlipAcc));
                    }
                }
            }
        }
        public double MoveFlipDec
        {
            get { return _moveFlipDec; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveFlipDec != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.dec", value)) return;
                        if (!SetVPlusValue("fbl.mf.dec", value)) return;

                        _moveFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveFlipDec));
                    }
                }
                else
                {
                    if (_moveFlipDec != value)
                    {
                        _moveFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveFlipDec));
                    }
                }
            }
        }
        public double MoveFlipSpeed
        {
            get { return _moveFlipSpeed; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveFlipSpeed != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.speed", value)) return;
                        if (!SetVPlusValue("fbl.mf.speed", value)) return;

                        _moveFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveFlipSpeed));
                    }
                }
                else
                {
                    if (_moveFlipSpeed != value)
                    {
                        _moveFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveFlipSpeed));
                    }
                }
            }
        }
        public double MoveFlipDelay
        {
            get { return _moveFlipDelay; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveFlipDelay != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.delay", value)) return;
                        if (!SetVPlusValue("fbl.mf.delay", value)) return;

                        _moveFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveFlipDelay));
                    }
                }
                else
                {
                    if (_moveFlipDelay != value)
                    {
                        _moveFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveFlipDelay));
                    }
                }
            }
        }
        public double MoveFlipCount
        {
            get { return _moveFlipCount; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveFlipCount != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mf.count", value)) return;
                        if (!SetVPlusValue("fbl.mf.count", value)) return;

                        _moveFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveFlipCount));
                    }
                }
                else
                {
                    if (_moveFlipCount != value)
                    {
                        _moveFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveFlipCount));
                    }
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
                if (Controller.IsAlive)
                {
                    if (value != _moveAngle)
                    {
                        if (!RecipeManagerUpdate("fbl.m.angle", value)) return;
                        if (!SetVPlusValue("fbl.m.angle", value)) return;

                        _moveAngle = value;
                        this.OnPropertyChanged(nameof(MoveAngle));
                    }
                }
                else
                {
                    if (_moveAngle != value)
                    {
                        _moveAngle = value;
                        this.OnPropertyChanged(nameof(MoveAngle));
                    }
                }
            }
        }
        public double MoveAcc
        {
            get { return _moveAcc; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveAcc != value) 
                    {
                        if (!RecipeManagerUpdate("fbl.m.acc", value)) return;
                        if (!SetVPlusValue("fbl.m.acc", value)) return;

                        _moveAcc = value;
                        this.OnPropertyChanged(nameof(MoveAcc));
                    }
                }
                else
                {
                    if (_moveAcc != value)
                    {
                        _moveAcc = value;
                        this.OnPropertyChanged(nameof(MoveAcc));
                    }
                }
            }
        }
        public double MoveDec
        {
            get { return _moveDec; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveDec != value)
                    {
                        if (!RecipeManagerUpdate("fbl.m.dec", value)) return;
                        if (!SetVPlusValue("fbl.m.dec", value)) return;

                        _moveDec = value;
                        this.OnPropertyChanged(nameof(MoveDec));
                    }
                }
                else
                {
                    if (_moveDec != value)
                    {
                        _moveDec = value;
                        this.OnPropertyChanged(nameof(MoveDec));
                    }
                }
            }
        }
        public double MoveSpeed
        {
            get { return _moveSpeed; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (value != _moveSpeed)
                    {
                        if (!RecipeManagerUpdate("fbl.m.speed", value)) return;
                        if (!SetVPlusValue("fbl.m.speed", value)) return;

                        _moveSpeed = value;
                        this.OnPropertyChanged(nameof(MoveSpeed));
                    }
                }
                else
                {
                    if (_moveSpeed != value)
                    {
                        _moveSpeed = value;
                        this.OnPropertyChanged(nameof(MoveSpeed));
                    }
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
                if (Controller.IsAlive)
                {                     
                    if (_flipDelay != value)
                    {
                        if (!RecipeManagerUpdate("fbl.f.delay", value)) return;
                        if (!SetVPlusValue("fbl.f.delay", value)) return;

                        _flipDelay = value;
                        this.OnPropertyChanged(nameof(FlipDelay));
                        //this.UpdateDisplay();
                    }
                }
                else
                {
                    if (_flipDelay != value)
                    {
                        _flipDelay = value;
                        this.OnPropertyChanged(nameof(FlipDelay));
                    }
                }
            }
        }
        public double FlipCount
        {
            get { return _flipCount; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_flipCount != value)
                    {                   
                        if (!RecipeManagerUpdate("fbl.f.count", value)) return;                       
                        if (!SetVPlusValue("fbl.f.count", value)) return;

                        _flipCount = value;
                        this.OnPropertyChanged(nameof(FlipCount));
                    }
                }
                else
                {
                    if (_flipCount != value)
                    {
                        _flipCount = value;
                        this.OnPropertyChanged(nameof(FlipCount));
                    }
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
                if (Controller.IsAlive)
                {
                    if (_moveBlowAngle != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mb.angle", value)) return;                      
                        if (!SetVPlusValue("fbl.mb.angle", value)) return;

                        _moveBlowAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowAngle));
                    }
                }
                else
                {
                    if (_moveBlowAngle != value)
                    {
                        _moveBlowAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowAngle));
                    }
                }
            }
        }
        public double MoveBlowAcc
        {
            get { return _moveBlowAcc; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowAcc != value)
                    {
                         if (!RecipeManagerUpdate("fbl.mb.acc", value)) return;                       
                        if (!SetVPlusValue("fbl.mb.acc", value)) return;

                        _moveBlowAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowAcc));
                    }
                }
                else
                {
                    if (_moveBlowAcc != value)
                    {
                        _moveBlowAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowAcc));
                    }
                }
            }
        }
        public double MoveBlowDec
        {
            get { return _moveBlowDec; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowDec != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mb.dec", value)) return;                       
                        if (!SetVPlusValue("fbl.mb.dec", value)) return;

                        _moveBlowDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowDec));
                    }
                }
                else
                {
                    if (_moveBlowDec != value)
                    {
                        _moveBlowDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowDec));
                    }
                }
            }
        }
        public double MoveBlowSpeed
        {
            get { return _moveBlowSpeed; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowSpeed != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mb.speed", value)) return;
                        if (!SetVPlusValue("fbl.mb.speed", value)) return;

                        _moveBlowSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowSpeed));
                    }
                }
                else
                {
                    if (_moveBlowSpeed != value)
                    {
                        _moveBlowSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowSpeed));
                    }
                }
            }
        }
        public double MoveBlowTime
        {
            get { return _moveBlowTime; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowTime != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mb.time", value)) return;                       
                        if (!SetVPlusValue("fbl.mb.time", value)) return;

                        _moveBlowTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowTime));
                    }
                }
                else
                {
                    if (_moveBlowTime != value)
                    {
                        _moveBlowTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowTime));
                    }
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
                if (Controller.IsAlive)
                {
                    if (_blowTime != value) 
                    {
                        if (!RecipeManagerUpdate("fbl.b.time", value)) return;                        
                        if (!SetVPlusValue("fbl.b.time", value)) return;

                        _blowTime = value;
                        this.OnPropertyChanged(nameof(BlowTime));
                    }
                }
                else
                {
                    if (_blowTime != value)
                    {
                        _blowTime = value;
                        this.OnPropertyChanged(nameof(BlowTime));
                    }
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
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipAngle != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.angle", value)) return;                        
                        if (!SetVPlusValue("fbl.mbf.angle", value)) return;

                        _moveBlowFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAngle));
                        this.UpdateDisplay();
                    }
                }
                else
                {
                    if (_moveBlowFlipAngle != value)
                    {
                        _moveBlowFlipAngle = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAngle));
                    }
                }

            }
        }
        public double MoveBlowFlipAcc
        {
            get { return _moveBlowFlipAcc; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipAcc != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.acc", value)) return;
                        if (!SetVPlusValue("fbl.mbf.acc", value)) return;

                        _moveBlowFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAcc));
                    }
                }
                else
                {
                    if (_moveBlowFlipAcc != value)
                    {
                        _moveBlowFlipAcc = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipAcc));
                    }
                }
            }
        }
        public double MoveBlowFlipDec
        {
            get { return _moveBlowFlipDec; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipDec != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.dec", value)) return;
                        if (!SetVPlusValue("fbl.mbf.dec", value)) return;

                        _moveBlowFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDec));
                    }
                }
                else
                {
                    if (_moveBlowFlipDec != value)
                    {
                        _moveBlowFlipDec = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDec));
                    }
                }
            }
        }
        public double MoveBlowFlipSpeed
        {
            get { return _moveBlowFlipSpeed; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipSpeed != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.speed", value)) return;
                        if (!SetVPlusValue("fbl.mbf.speed", value)) return;

                        _moveBlowFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipSpeed));
                    }
                }
                else
                {
                    if (_moveBlowFlipSpeed != value)
                    {
                        _moveBlowFlipSpeed = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipSpeed));
                    }
                }
            }
        }
        public double MoveBlowFlipDelay
        {
            get { return _moveBlowFlipDelay; }
            set
            {
                if(Controller.IsAlive)
                {
                    if ((_moveBlowFlipDelay != value) && (Controller.IsAlive))
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.delay", value)) return;                        
                        if (!SetVPlusValue("fbl.mbf.delay", value)) return;

                        _moveBlowFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDelay));
                    }
                }
                else
                {
                    if (_moveBlowFlipDelay != value)
                    {
                        _moveBlowFlipDelay = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipDelay));
                    }
                }
            }
        }
        public double MoveBlowFlipCount
        {
            get { return _moveBlowFlipCount; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipCount != value)
                    {
                        if (!RecipeManagerUpdate("fbl.mbf.count", value)) return;                        
                        if (!SetVPlusValue("fbl.mbf.count", value)) return;

                        _moveBlowFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipCount));
                    }
                }
                else
                {
                    if (_moveBlowFlipCount != value)
                    {
                        _moveBlowFlipCount = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipCount));
                    }
                }
            }
        }
        public double MoveBlowFlipTime
        {
            get { return _moveBlowFlipTime; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_moveBlowFlipTime != value) 
                    {     
                        if (!RecipeManagerUpdate("fbl.mbf.time", value)) return;                        
                        if (!SetVPlusValue("fbl.mbf.time", value)) return;

                        _moveBlowFlipTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipTime));
                    }
                }
                else
                {
                    if (_moveBlowFlipTime != value)
                    {
                        _moveBlowFlipTime = value;
                        this.OnPropertyChanged(nameof(MoveBlowFlipTime));
                    }
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
                if (Controller.IsAlive)
                {
                    if (_shakeCWAngle != value)
                    {
                        if (!RecipeManagerUpdate("fbl.sh.cw", value)) return;                       
                        if (!SetVPlusValue("fbl.sh.cw", value)) return;


                        _shakeCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCWAngle));
                    }
                }
                else
                {
                    if (_shakeCWAngle != value)
                    {
                        _shakeCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCWAngle));
                    }
                }
            }
        }
        public double ShakeCCWAngle
        {
            get { return _shakeCCWAngle; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_shakeCCWAngle != value) 
                    {                        
                        if (!RecipeManagerUpdate("fbl.sh.ccw", value)) return;                        
                        if (!SetVPlusValue("fbl.sh.ccw", value)) return;

                        _shakeCCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCCWAngle));
                    }
                }
                else
                {
                    if (_shakeCCWAngle != value)
                    {
                        _shakeCCWAngle = value;
                        this.OnPropertyChanged(nameof(ShakeCCWAngle));
                    }
                }
            }
        }
        public double ShakeAcc
        {
            get { return _shakeAcc; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_shakeAcc != value)
                    {
                        if (!RecipeManagerUpdate("fbl.sh.acc", value)) return;                        
                        if (!SetVPlusValue("fbl.sh.acc", value)) return;

                        _shakeAcc = value;
                        this.OnPropertyChanged(nameof(ShakeAcc));
                    }
                }
                else
                {
                    if (_shakeAcc != value)
                    {
                        _shakeAcc = value;
                        this.OnPropertyChanged(nameof(ShakeAcc));
                    }
                }
            }
        }
        public double ShakeDec
        {
            get { return _shakeDec; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_shakeDec != value)
                    {
                        if (!RecipeManagerUpdate("fbl.sh.dec", value)) return;                        
                        if (!SetVPlusValue("fbl.sh.dec", value)) return;

                        _shakeDec = value;
                        this.OnPropertyChanged(nameof(ShakeDec));
                    }
                }
                else
                {
                    if (_shakeDec != value)
                    {
                        _shakeDec = value;
                        this.OnPropertyChanged(nameof(ShakeDec));
                    }
                }
            }
        }
        public double ShakeSpeed
        {
            get { return _shakeSpeed; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_shakeSpeed != value) 
                    {                        
                        if (!RecipeManagerUpdate("fbl.sh.speed", value)) return;                        
                        if (!SetVPlusValue("fbl.sh.speed", value)) return;

                        _shakeSpeed = value;
                        this.OnPropertyChanged(nameof(ShakeSpeed));
                    }
                }
                else
                {
                    if (_shakeSpeed != value)
                    {
                        _shakeSpeed = value;
                        this.OnPropertyChanged(nameof(ShakeSpeed));
                    }
                }
            }
        }
        public double ShakeCount
        {
            get { return _shakeCount; }
            set
            {
                if (Controller.IsAlive)
                {
                    if (_shakeCount != value)
                    {
                        if (!RecipeManagerUpdate("fbl.sh.count", value)) return;                       
                        if (!SetVPlusValue("fbl.sh.count", value)) return;

                        _shakeCount = value;
                        this.OnPropertyChanged(nameof(ShakeCount));
                    }
                }
                else
                {
                    if (_shakeCount != value)
                    {
                        _shakeCount = value;
                        this.OnPropertyChanged(nameof(ShakeCount));
                    }
                }
            }
        }
        #endregion FlexiBowl Parameter Properties


        #region Flexibowl DelegateCommand Properties

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


        #endregion Flexibowl DelegateCommand Properties


        #endregion Properties


        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="connectionHelper"></param>
        public ControllerViewModel(INameLookupService nameLookupService, IAdeptController controller, IConnectionHelper connectionHelper):base(controller as IAceObject)
        {
            this.NameLookupService = nameLookupService;

            // Assign method to Action Delegate
            LogMethodDelegate = LogToFile;

            // Assign method to Action-Delegate
            BackGroundMonitorDelegate = BackgroundMonitorMethod;

            // Instantiate background monitor, handle over deleagte 
            this.backgroundMonitor = new BackgroundCommandMonitor(BackGroundMonitorDelegate);

            this.ConnectionHelper = connectionHelper;

            // Instantiate Flexibowl Delegate Commands (used in Command Binding to View)
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


        #region Flexibowl DelegateCommands (Method Implementations)

        /// <summary>
        /// All Flexibowl Commands work together with the V+ program "fb.comm()", which is responsible 
        /// to send messages to the Applied Motion STAC5 stepper motor driver over TCP/IP. 
        /// 
        /// The flow of this V+ user task is controlled using the V+ variable 'fb.cmd' (FlexiBowl Command) 
        /// and the parameters for the commands are written to the V+ array fb.arg[]. 
        /// 
        /// This C# application uses the 'Link' class to remotely manipulate the V+ variables from here.
        /// 
        ///    * fb.cmd - defines the Flexibowl commands in V+ (see V+ program fb.init() for details)
        ///    
        ///         * 'fb.param.set' causes the V+ program fb.set_param() to run
        ///           In fb.set_param() all non-zero fb.arg[] values get communicated to the STAC5 motor driver registers,
        ///           to be used in subsequent commands, like listed below.
        ///           
        ///         * Each of the other commands listed below eXecute a so called 'Q-Program' (--> "QXnn") inside the 
        ///           STAC5 driver, acting on the parameters written to the driver registers before (see above)
        ///            _fbMove          - "QX2" 
        ///            _fbMoveFlip      - "QX3"
        ///            _fbFlip          - "QX10"
        ///            _fbMoveBlow      - "QX5"
        ///            _fbBlow          - "QX9"
        ///            _fbMoveBlowFlip  - "QX4"
        ///            _fbShake         - "QX6"
        ///            _fbLightOn       - "QX7"
        ///            _fbLightOff      - "QX8"
        ///            _fbQickEmpty     - "QX11"
        ///            _fbResetAlarm    - "QX12"

        
        /// </summary>
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
                    // Set Parameter for MoveFlip Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", MoveFlipAcc);
                    Controller.Link.SetR("fb.arg[2]", MoveFlipDec);
                    Controller.Link.SetR("fb.arg[3]", MoveFlipSpeed);
                    Controller.Link.SetR("fb.arg[4]", MoveFlipAngle);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", MoveFlipCount);
                    Controller.Link.SetR("fb.arg[12]", MoveFlipDelay);
                    Controller.Link.SetR("fb.arg[13]", 0);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set MoveFlip Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0) 
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set MoveFlip Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbMoveFlip);
                    LogToFile("Execute MoveFlip Command "+ Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0) 
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute MoveFlip Command done");
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
                    //LogToFile("Move Command");

                    // Set Parameter for MoveFlip Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", MoveAcc);
                    Controller.Link.SetR("fb.arg[2]", MoveDec);
                    Controller.Link.SetR("fb.arg[3]", MoveSpeed);
                    Controller.Link.SetR("fb.arg[4]", MoveAngle);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", 0);
                    Controller.Link.SetR("fb.arg[12]", 0);
                    Controller.Link.SetR("fb.arg[13]", 0);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set Move Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set Move Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbMove);
                    LogToFile("Execute Move Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute Move Command done");
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
                    //LogToFile("Flip Command");

                    // Set Parameter for Flip Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", 0);
                    Controller.Link.SetR("fb.arg[2]", 0);
                    Controller.Link.SetR("fb.arg[3]", 0);
                    Controller.Link.SetR("fb.arg[4]", 0);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", FlipCount);
                    Controller.Link.SetR("fb.arg[12]", FlipDelay);
                    Controller.Link.SetR("fb.arg[13]", 0);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set Flip Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set Move Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbFlip);
                    LogToFile("Execute Flip Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute Flip Command done");
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
                    //LogToFile("MoveBlow Command");

                    // Set Parameter for MoveBlow Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", MoveBlowAcc);
                    Controller.Link.SetR("fb.arg[2]", MoveBlowDec);
                    Controller.Link.SetR("fb.arg[3]", MoveBlowSpeed);
                    Controller.Link.SetR("fb.arg[4]", MoveBlowAngle);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", 0);
                    Controller.Link.SetR("fb.arg[12]", 0);
                    Controller.Link.SetR("fb.arg[13]", MoveBlowTime);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set MoveBlow Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set MoveBlow Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbMoveBlow);
                    LogToFile("Execute MoveBlow Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute MoveBlow Command done");
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

                    // Set Parameter for Blow Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", 0);
                    Controller.Link.SetR("fb.arg[2]", 0);
                    Controller.Link.SetR("fb.arg[3]", 0);
                    Controller.Link.SetR("fb.arg[4]", 0);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", 0);
                    Controller.Link.SetR("fb.arg[12]", 0);
                    Controller.Link.SetR("fb.arg[13]", BlowTime);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set Blow Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set Blow Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbBlow);
                    LogToFile("Execute Blow Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute Blow Command done");
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
                    //LogToFile("MoveBlowFlip Command");

                    // Set Parameter for MoveBlowFlip Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", MoveBlowFlipAcc);
                    Controller.Link.SetR("fb.arg[2]", MoveBlowFlipDec);
                    Controller.Link.SetR("fb.arg[3]", MoveBlowFlipSpeed);
                    Controller.Link.SetR("fb.arg[4]", MoveBlowFlipAngle);
                    Controller.Link.SetR("fb.arg[5]", 0);
                    Controller.Link.SetR("fb.arg[6]", 0);
                    Controller.Link.SetR("fb.arg[7]", 0);
                    Controller.Link.SetR("fb.arg[8]", 0);
                    Controller.Link.SetR("fb.arg[9]", 0);
                    Controller.Link.SetR("fb.arg[10]", 0);
                    Controller.Link.SetR("fb.arg[11]", MoveBlowFlipCount);
                    Controller.Link.SetR("fb.arg[12]", MoveBlowFlipDelay);
                    Controller.Link.SetR("fb.arg[13]", MoveBlowFlipTime);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set MoveBlowFlip Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set MoveBlowFlip Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbMoveBlowFlip);
                    LogToFile("Execute MoveBlowFlip Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute MoveBlowFlip Command done");
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
                    //LogToFile("Shake Command");

                    // Set Parameter for Shake Command ('0' means "skip modifying register in FlexiBowl drive in V+ program fb.set_param()") 
                    Controller.Link.SetR("fb.arg[1]", 0);
                    Controller.Link.SetR("fb.arg[2]", 0);
                    Controller.Link.SetR("fb.arg[3]", 0);
                    Controller.Link.SetR("fb.arg[4]", 0);
                    Controller.Link.SetR("fb.arg[5]", ShakeAcc);
                    Controller.Link.SetR("fb.arg[6]", ShakeDec);
                    Controller.Link.SetR("fb.arg[7]", ShakeCount);
                    Controller.Link.SetR("fb.arg[8]", ShakeCWAngle);
                    Controller.Link.SetR("fb.arg[9]", ShakeCCWAngle);
                    Controller.Link.SetR("fb.arg[10]", ShakeSpeed);
                    Controller.Link.SetR("fb.arg[11]", 0);
                    Controller.Link.SetR("fb.arg[12]", 0);
                    Controller.Link.SetR("fb.arg[13]", 0);

                    Controller.Link.SetR("fb.cmd", _fbParamSet);
                    LogToFile("Set Shake Parameter " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Set Shake Parameter done.");

                    Controller.Link.SetR("fb.cmd", _fbShake);
                    LogToFile("Execute Shake Command " + Controller.Link.ListR("fb.cmd"));
                    while (Controller.Link.ListR("fb.cmd") != 0)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    //LogToFile("Execute Shake Command done");
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


                    if (FlexiBowlBackLight == -1)
                    {
                        LogToFile("Light Command -1");
                        FlexiBowlBackLight = 0;
                    }
                    else
                    {
                        LogToFile("Light Command 0");
                        FlexiBowlBackLight = -1;
                    }
                    UpdateDisplay();
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

        #endregion Flexibowl DelegateCommands - method implementations


        /// <summary>
        /// Refresh the complete view
        /// </summary>
        private void UpdateDisplay()
        {
            var dispatcher = System.Windows.Application.Current.Dispatcher;
            try {
                dispatcher?.Invoke(() => {

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

                    this.OnPropertyChanged("FlexiBowlBackLightButtonText");
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
        /// Set V+ Double in Controller Memory
        /// </summary>
        /// <param name="vPlusVariableName"></param>
        /// <param name="previous"></param>
        /// <param name="value"></param>
        /// <param name="success"></param>
        private bool SetVPlusValue(string vPlusVariableName, double value)
        {
            //try
            //{
            //    LogToFile("V+ Memory Update:     " + vPlusVariableName + " from " + previous.ToString() + " to "+ value.ToString());

            //    var link = this.Controller.Link;
            //    link.SetR(vPlusVariableName, value);
            //    success = true;
            //}
            //catch (Exception ex)
            //{
            //    LogToFile("SetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
            //    OnReportError("** SetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString()+" **");
            //    success = false;
            //}
            try
            {
                var link = Controller?.Link;
                if (link != null)
                {
                    var isDefined = link.ListR(string.Format("DEFINED({0})", vPlusVariableName));
                    if (isDefined != 0)
                    {
                        double previous = Controller.GetRealValue(vPlusVariableName);
                        LogToFile("V+ Memory Update:     Change " + vPlusVariableName + " from " + previous.ToString() + " to " + value.ToString());
                    } 
                    else  
                    {
                        LogToFile("V+ Memory Update:     Create " + vPlusVariableName + " as " + value.ToString());
                    }
                    link.SetR(vPlusVariableName, value);
                }
            }
            catch (Exception ex)
            {
                LogToFile("SetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
                OnReportError("** SetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString() + "**");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get V+ Double from Controller Memory. Skip, for selected TextBox.
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
            }
            catch (Exception ex)
            {
                LogToFile("GetVPlusValue exception: " + vPlusVariableName + " " + ex.ToString());
                OnReportError("** GetVPlusValue  exception: " + vPlusVariableName + " " + ex.ToString() + "**");
            }
        }

        /// <summary>
        /// BackgroundCommandMonitor - Implementation of the Action-delegate 'BackgroundMonitorDelegate' from 'PropertyModifiedBase'. 
        /// Reads complete list of V+ Doubles in Custom UI and persists active recipe to text file via 'LastActiveRecipe' property.
        /// </summary>
        private void BackgroundMonitorMethod()
        {
            if (Controller.IsAlive)
            {
                if (recipeManager == null) { recipeManager = RecipeManagerGetReference(); };
                //var activeRecipe = recipeManager.ActiveRecipe;
                if (recipeManager.ActiveRecipe != null)
                {
                    var recipeToken = recipeManager.ActiveRecipe.CreateRecipeReference();
                    using (recipeToken)
                    {
                        if (LastActiveRecipe != recipeToken.Recipe.Name)
                        {
                            LastActiveRecipe = recipeToken.Recipe.Name;
                            LogToFile("Background Task: Update new 'LastActiveRecipe' with '" + recipeToken.Recipe.Name+"'");
                        }
                    }
                }
                else
                {
                    if (LastActiveRecipe != "New FlexiBowl Recipe")
                        LastActiveRecipe = "New FlexiBowl Recipe";
                }
 
                GetVPlusValue("fbl.backlight", nameof(FlexiBowlBackLight), (a) => FlexiBowlBackLight = a);
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
                // Controller not connected

                // Zero all Flexibowl Properties in the UI
                FlexiBowlBackLight = 0;
                MoveFlipAngle = 0;
                MoveFlipAcc = 0;
                MoveFlipDec = 0;
                MoveFlipSpeed = 0;
                MoveFlipDelay = 0;
                MoveFlipCount = 0;
                MoveAngle = 0;
                MoveAcc = 0;
                MoveDec = 0;
                MoveSpeed = 0;
                FlipDelay = 0;
                FlipCount = 0;
                MoveBlowAngle = 0;
                MoveBlowAcc = 0;
                MoveBlowDec = 0;
                MoveBlowSpeed = 0;
                MoveBlowTime = 0;
                BlowTime = 0;
                MoveBlowFlipAngle = 0;
                MoveBlowFlipAcc = 0;
                MoveBlowFlipDec = 0;
                MoveBlowFlipSpeed = 0;
                MoveBlowFlipDelay = 0;
                MoveBlowFlipCount = 0;
                MoveBlowFlipTime = 0;
                ShakeCWAngle = 0;
                ShakeCCWAngle = 0;
                ShakeAcc = 0;
                ShakeDec = 0;
                ShakeSpeed = 0;
                ShakeCount = 0;

                // Disable Command Buttons
                this.UpdateDisplay();


                System.Threading.Thread.Sleep(300);
            }

            return;
        }

        /// <summary>
        /// Update V+ Variable in currently selected Recipe
        /// </summary>
        /// <param name="vplusVariableName"></param>
        /// <param name="previous"></param>
        /// <param name="value"></param>
        /// <param name="success"></param>
        private bool RecipeManagerUpdate(string vplusVariableName, double value)
        {
            if (!Controller.IsAlive)
            {
                LogToFile("** Update Recipe: No Controller Connection **");
                OnReportError("** Cannot update recipe without Controller Connection **");
                return false;
            }

            // Get the RecipeManger reference
            if (recipeManager == null) {
                recipeManager = RecipeManagerGetReference();
                if (recipeManager == null)
                {
                    LogToFile("** Update Recipe: No Recipe Manager reference **");
                    OnReportError("** Cannot update recipe without Recipe Manager reference **");
                    return false;
                }
            };

            // Enforce activate recipe to store FlexiBowl-data in
            if (!RecipeSelect(recipeManager))
            {
                LogToFile("** Update Recipe: No Recipe selected in RecipeManager "+ recipeManager.Name + " **");
                OnReportError("** Cannot update recipe without a recipe selected in RecipeManager " + recipeManager.Name + " **");
                return false;
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
            else
            {
                if (configuration == null)
                {
                    LogToFile("  no 'VariableHandle' to write '" + vplusVariableName + "' to");
                    return true;
                }
            }

            // Store this value into the component of the active recipe
            var variable = Controller.Memory.Variables.GetVariableByName(vplusVariableName);
            var activeRecipe = recipeManager.ActiveRecipe;
            if (activeRecipe != null)
            {
                var recipeToken = recipeManager.ActiveRecipe.CreateRecipeReference();
                using (recipeToken)
                {
                    var recipe = recipeToken.Recipe;

                    var component = recipe.Components.Where(c => c is IVPlusGlobalVariableCollectionRecipeComponent).FirstOrDefault() as IVPlusGlobalVariableCollectionRecipeComponent;
                    if(component == null)
                    {
                        LogToFile(recipe.Name + " has no 'VPlusGlobalVariableCollectionRecipeComponent' to write '" + vplusVariableName + "' to");
                        return true;
                    }



                    double previous = 0;
                    if (variable != null)
                    {
                        LogToFile(" previous from recipe is " + previous.ToString());
                        previous = variable.InitialRealValue;
                    }
                    else
                    {
                        previous = Controller.Link.ListR(vplusVariableName);
                        LogToFile(" previous from recipe not existing, get from controller:" + previous.ToString());
                    }



                    LogToFile(recipe.Name + ": " + variable.Name + " from " + previous.ToString() + " to " + value.ToString());

                    component.SetRealValue(variable, value);
                }
            }

            return true;
        }

        /// <summary>
        /// Return a reference to a RecipeManager
        /// </summary>
        /// <returns> recipeManager </returns>
        public IRecipeManager RecipeManagerGetReference()
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
        private bool RecipeSelect(IRecipeManager recipeManager)
        {
            if (recipeManager.ActiveRecipe == null)
            {
                if (File.Exists(aceAppDataFolder + @"\LastActiveRecipe.txt"))
                {
                    // Try loading name of last active recipe from '%APPDATA%\OMRON\ACE\LastActiveRecipe.txt'
                    LastActiveRecipe = System.IO.File.ReadAllText(aceAppDataFolder + @"\LastActiveRecipe.txt");
                    LogToFile(recipeManager.Name + ": Found last active recipe '"+ LastActiveRecipe+ "' in file '" + aceAppDataFolder + @"\LastActiveRecipe.txt'");
                }
                else
                {
                    // No previously selected recipe found --> default to 'New Flexibowl Recipe'
                    // Either select existing from list of available recipes, or create a 'New Flexibowl Recipe'
                    LastActiveRecipe = "New Flexibowl Recipe";
                    LogToFile(recipeManager.Name + ": Select or create '" + LastActiveRecipe + "'");
                }

                // Scan list of available recipes and activate LastActiveRecipe if available.
                storedRecipeFound = false;
                if (recipeManager.Recipes.Length > 0)
                {
                    foreach (IRecipeToken r in recipeManager.Recipes)
                    {
                        using (IRecipeReference rRecipeReference = r.CreateRecipeReference())
                        {
                            IRecipe rRecipe = rRecipeReference.Recipe;
                            if (rRecipe.Name.Contains(LastActiveRecipe))
                            {
                                LogToFile("Select existing '"+rRecipe.Name+"' from list of available recipes");
                                storedRecipeFound = true;
                                recipeManager.SelectRecipe(r, true);
                                break;
                            }
                        }
                    }
                }

                // LastActiveRecipe NOT found in list of available recipes - create with default values and activate.
                if (!storedRecipeFound)
                {
                    // Create and select 'New FlexiBowl Recipe'
                    LogToFile("No '"+LastActiveRecipe+"' found in available recipes list. Will create and select it.");
                    RecipeCreateNewAndSelect(recipeManager, LastActiveRecipe);


                    //var configuration = recipeManager.Configurations.FirstOrDefault(c => c is IVPlusGlobalVariableCollectionRecipeConfiguration) as IVPlusGlobalVariableCollectionRecipeConfiguration;
                    //if (configuration == null)
                    //{
                    //    LogToFile("  no 'VPlusGlobalVariableCollectionRecipeComponent' to write to");
                    //    return true;
                    //}


                    var activeRecipe = recipeManager.ActiveRecipe;
                    if (activeRecipe != null)
                    {


                        var recipeToken = recipeManager.ActiveRecipe.CreateRecipeReference();
                        using (recipeToken)
                        {
                            var recipe = recipeToken.Recipe;
                            var component = recipe.Components.Where(c => c is IVPlusGlobalVariableCollectionRecipeComponent).FirstOrDefault() as IVPlusGlobalVariableCollectionRecipeComponent;

;
                        }
                    }


                    // For the new recipe, enforce default values in recipe and V+ memory using the properties 
                    FlexiBowlBackLight = 0;
                    MoveFlipAngle = 45;
                    MoveFlipAcc = 250;
                    MoveFlipDec = 250;
                    MoveFlipSpeed = 250;
                    MoveFlipDelay = 50;
                    MoveFlipCount = 2;
                    MoveAngle = 45;
                    MoveAcc = 250;
                    MoveDec = 250;
                    MoveSpeed = 250;
                    FlipDelay = 50;
                    FlipCount = 2;
                    MoveBlowAngle = 45;
                    MoveBlowAcc = 250;
                    MoveBlowDec = 250;
                    MoveBlowSpeed = 1250;
                    MoveBlowTime = 200;
                    BlowTime = 200;
                    MoveBlowFlipAngle = 45;
                    MoveBlowFlipAcc = 250;
                    MoveBlowFlipDec = 250;
                    MoveBlowFlipSpeed = 250;
                    MoveBlowFlipDelay = 50;
                    MoveBlowFlipCount = 2;
                    MoveBlowFlipTime = 200;
                    ShakeCWAngle = 45;
                    ShakeCCWAngle = -45;
                    ShakeAcc = 250;
                    ShakeDec = 250;
                    ShakeSpeed = 250;
                    ShakeCount = 2;
                }

            }
            return true;
        }

        /// <summary>
        /// Create new recipe "Recipe 1", 
        /// - rename it to "New FlexiBowl Recipe", 
        /// - select as active and 
        /// - write default values into the components
        /// </summary>
        /// <param name="recipeManager"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private void RecipeCreateNewAndSelect(IRecipeManager recipeManager, string name)
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

                // For the new recipe, enforce default values in recipe and V+ memory using the properties 
                //FlexiBowlBackLight = 0;
                //MoveFlipAngle = 45;
                //MoveFlipAcc = 250;
                //MoveFlipDec = 250;
                //MoveFlipSpeed = 250;
                //MoveFlipDelay = 50;
                //MoveFlipCount = 2;
                //MoveAngle = 45;
                //MoveAcc = 250;
                //MoveDec = 250;
                //MoveSpeed = 250;
                //FlipDelay = 50;
                //FlipCount = 2;
                //MoveBlowAngle = 45;
                //MoveBlowAcc = 250;
                //MoveBlowDec = 250;
                //MoveBlowSpeed = 1250;
                //MoveBlowTime = 200;
                //BlowTime = 200;
                //MoveBlowFlipAngle = 45;
                //MoveBlowFlipAcc = 250;
                //MoveBlowFlipDec = 250;
                //MoveBlowFlipSpeed = 250;
                //MoveBlowFlipDelay = 50;
                //MoveBlowFlipCount = 2;
                //MoveBlowFlipTime = 200;
                //ShakeCWAngle = 45;
                //ShakeCCWAngle = -45;
                //ShakeAcc = 250;
                //ShakeDec = 250;
                //ShakeSpeed = 250;
                //ShakeCount = 2;

                //SetVPlusValue("fbl.backlight", 0);
                //SetVPlusValue("fbl.m.acc", 250);
                //SetVPlusValue("fbl.m.dec", 250);
                //SetVPlusValue("fbl.m.speed", 250);
                //SetVPlusValue("fbl.m.angle", 45);
                //SetVPlusValue("fbl.sh.acc", 250);
                //SetVPlusValue("fbl.sh.dec", 250);
                //SetVPlusValue("fbl.sh.count", 2);
                //SetVPlusValue("fbl.sh.cw", 45);
                //SetVPlusValue("fbl.sh.ccw", -45);
                //SetVPlusValue("fbl.sh.speed", 250);
                //SetVPlusValue("fbl.f.count", 2);
                //SetVPlusValue("fbl.f.delay", 50);
                //SetVPlusValue("fbl.b.time", 200);
                //SetVPlusValue("fbl.mf.acc", 250);
                //SetVPlusValue("fbl.mf.dec", 250);
                //SetVPlusValue("fbl.mf.speed", 250);
                //SetVPlusValue("fbl.mf.angle", 45);
                //SetVPlusValue("fbl.mf.count", 2);
                //SetVPlusValue("fbl.mf.delay", 50);
                //SetVPlusValue("fbl.mb.acc", 250);
                //SetVPlusValue("fbl.mb.dec", 250);
                //SetVPlusValue("fbl.mb.speed", 250);
                //SetVPlusValue("fbl.mb.angle", 45);
                //SetVPlusValue("fbl.mb.time", 200);
                //SetVPlusValue("fbl.mbf.acc", 250);
                //SetVPlusValue("fbl.mbf.dec", 250);
                //SetVPlusValue("fbl.mbf.speed", 250);
                //SetVPlusValue("fbl.mbf.angle", 45);
                //SetVPlusValue("fbl.mbf.count", 2);
                //SetVPlusValue("fbl.mbf.delay", 50);
                //SetVPlusValue("fbl.mbf.time", 200);
            }
            catch (Exception ex)
            {
                LogToFile("Exception when creating new recipe: " + ex.Message);
                return;
            }
            return;
        }

        /// <summary>
        /// Type-Debugging into text-file. Implementation of method, assigned to LogMethodDelegate Action-Delegate.
        /// </summary>
        /// <param name="text"></param>
        static void LogToFile(string text)
        {
            lock (lockObject)
            {
                System.IO.File.AppendAllText(aceAppDataFolder + @"\DebugCustomUI.txt", text + Environment.NewLine);
            }

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

        public override void UnhookEvents() {
            base.UnhookEvents();
            backgroundMonitor.Stop();
        }
    }



    //interface IMultiThreadFileWriter
    //{
    //    // see https://softwareengineering.stackexchange.com/questions/177649/what-is-constructor-injection
    //    void WriteLine(string line);
    //}


    /// <summary>
    /// Class to use a ConcurrentQueue and a always running Task to accomplish accessing the shared Streamwriter-resource 
    /// to be accessed from one thread only (the task running in the background) and everyone else to deliver their payload 
    /// to a thread-safe queue.
    /// Background: In the ControllerViewModel class the DelegateCommand implementations use Task.Factory.StartNew(...).
    /// In the code getting started inside parallel threads we use logging to a file - which is not allowing simultaneous access
    /// from multiple threads.
    /// see https://briancaos.wordpress.com/2021/01/12/write-to-file-from-multiple-threads-async-with-c-and-net-core/ for details
    /// </summary>
    //public class MultiThreadFileWriter:IMultiThreadFileWriter
    //{
    //    private static ConcurrentQueue<string> _textToWrite = new ConcurrentQueue<string>();
    //    private CancellationTokenSource _source = new CancellationTokenSource();
    //    private CancellationToken _token;

    //    public MultiThreadFileWriter()
    //    {
    //        _token = _source.Token;
    //        // This is the task that will run
    //        // in the background and do the actual file writing
    //        Task.Run(WriteToFile, _token);
    //    }

    //    /// The public method where a thread can ask for a line
    //    /// to be written.
    //    public void WriteLine(string line)
    //    {
    //        _textToWrite.Enqueue(line);
    //    }

    //    /// The actual file writer, running
    //    /// in the background.
    //    private async void WriteToFile()
    //    {
    //        while (true)
    //        {
    //            if (_token.IsCancellationRequested)
    //            {
    //                return;
    //            }
    //            using (StreamWriter w = File.AppendText("c:\\myfile.txt"))
    //            {
    //                while (_textToWrite.TryDequeue(out string textLine))
    //                {
    //                    await w.WriteLineAsync(textLine);
    //                }
    //                w.Flush();
    //                Thread.Sleep(100);
    //            }
    //        }
    //    }
    //}
}