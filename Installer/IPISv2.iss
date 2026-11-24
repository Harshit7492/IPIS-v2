; ============================================================
; IPIS v2 - Inno Setup Installer
; Vardaan Electronics Industries Pvt Ltd
;
; Features:
; - Installs IPISv2 into Program Files
; - SELF-CONTAINED: the .NET runtime ships inside the app folder,
;   so target PCs need no .NET installed (nothing to download)
; - Build-time check: compile stops if the Publish folder is not a
;   self-contained x64 publish
; - Keeps the site's IPISv2.dll.config on upgrade
; - Uses same logo for Setup / Desktop / Start Menu
; - Desktop + Start Menu shortcuts launch IPISv2.exe directly
;   (the EXE manifest has requireAdministrator, so Windows
;    shows the normal UAC prompt)
; - Creates Scheduled Task for Windows startup
;     * Runs at user logon (10 second delay)
;     * Highest privileges (no UAC prompt at logon)
;     * Working directory = application folder
;     * Works on battery, no run-time limit
; - Removes Scheduled Task during uninstall
;
; Before compiling: publish IPISv2 SELF-CONTAINED into an EMPTY
; D:\Harshit workspace\Publish (Visual Studio -> Publish -> profile
; "SelfContained-win-x64"). Never publish framework-dependent into the
; same folder: leftover runtime DLLs make it look self-contained while
; the EXE still asks for an installed .NET. The checks below catch that.
; ============================================================


; ============================================================
; DEFINITIONS
; ============================================================

#define MyAppName "IPISv2"
#define MyAppVersion "1.0.0.6"
#define MyAppPublisher "Vardaan Electronics Industries Pvt Ltd"
#define MyAppExeName "IPISv2.exe"

; Scheduled Task Name
#define MyTaskName "IPISv2 Startup"

; Application / Setup Icon
#define MyIconFile "D:\Harshit workspace\IPISv2\Csproj\Images\logo.ico"

; Published application folder
#define PublishDir "D:\Harshit workspace\Publish"


; ============================================================
; BUILD-TIME CHECKS (stop the compile, never ship a broken setup)
; ============================================================

#if !FileExists(PublishDir + "\" + MyAppExeName)
  #error IPISv2.exe not found in D:\Harshit workspace\Publish. Publish the app first (see the note at the top of this script).
#endif

; A self-contained publish carries the runtime itself (coreclr.dll and
; hostfxr.dll sit next to the EXE). A framework-dependent publish does
; not, and would show "You must install or update .NET" on target PCs.
#if !FileExists(PublishDir + "\coreclr.dll") || !FileExists(PublishDir + "\hostfxr.dll")
  #error Publish folder is NOT self-contained (coreclr.dll / hostfxr.dll missing), so target PCs would need .NET installed. Publish self-contained into an empty Publish folder.
#endif

; The DLLs alone are not proof: a framework-dependent publish written over
; an old self-contained one leaves them behind, but IPISv2.exe then follows
; IPISv2.runtimeconfig.json and still asks for an installed .NET.
; Self-contained -> "includedFrameworks"; framework-dependent -> "frameworks".
#define RtCfg PublishDir + "\IPISv2.runtimeconfig.json"
#if !FileExists(RtCfg)
  #error IPISv2.runtimeconfig.json not found in D:\Harshit workspace\Publish. Publish the app first.
#endif
#define RtH FileOpen(RtCfg)
#if RtH == 0
  #error Could not open IPISv2.runtimeconfig.json in D:\Harshit workspace\Publish.
#endif
#define RtText ""
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#define RtText RtText + (FileEof(RtH) ? "" : FileRead(RtH))
#expr FileClose(RtH)
#if Pos("includedFrameworks", RtText) == 0
  #error Publish folder is framework-dependent (IPISv2.runtimeconfig.json has no includedFrameworks), so target PCs would need .NET installed. Publish self-contained into an empty Publish folder.
#endif

#if !FileExists(PublishDir + "\IPISv2.dll.config")
  #error IPISv2.dll.config not found in D:\Harshit workspace\Publish. The site config must be in the publish output.
#endif


; ============================================================
; SETUP
; ============================================================

[Setup]

; Unique Application ID
AppId={{85748E1A-1335-4A9F-BD98-3687005AF5B7}

AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
VersionInfoVersion={#MyAppVersion}

; Installation Location
DefaultDirName={autopf}\Vardaan Electronics Industries Pvt Ltd\{#MyAppName}

; Icons
SetupIconFile={#MyIconFile}
UninstallDisplayIcon={app}\images\logo.ico

; Architecture (the publish is win-x64)
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

; Administrator permission
PrivilegesRequired=admin

; Installer wizard
DisableProgramGroupPage=yes
WizardStyle=modern

; Output
OutputDir=D:\Harshit workspace\InstallerOutput
OutputBaseFilename=IPISv2_Setup
Compression=lzma2/max
SolidCompression=yes

; Prevent multiple setup instances
SetupMutex=IPISv2SetupMutex


; ============================================================
; LANGUAGES
; ============================================================

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"


; ============================================================
; FILES
; ============================================================

[Files]

; IPISv2 published application (self-contained: includes the .NET runtime).
; Everything except the site config, any test logs, and the 32-bit VLC
; libraries (the app is x64-only, so libvlc\win-x86 is never loaded).
Source: "{#PublishDir}\*"; \
    Excludes: "IPISv2.dll.config,\logs\*,\libvlc\win-x86\*"; \
    DestDir: "{app}"; \
    Flags: ignoreversion recursesubdirs createallsubdirs

; Site config (NMS IP, slave PC name, ...): written on the first install only,
; so an upgrade keeps each station's own settings.
Source: "{#PublishDir}\IPISv2.dll.config"; \
    DestDir: "{app}"; \
    Flags: onlyifdoesntexist

; Application logo
Source: "{#MyIconFile}"; \
    DestDir: "{app}\images"; \
    DestName: "logo.ico"; \
    Flags: ignoreversion


; ============================================================
; CLEAN UP FILES FROM OLDER VERSIONS
; ============================================================

[InstallDelete]
Type: files; Name: "{app}\IPISv2Launcher.vbs"


; ============================================================
; SHORTCUTS (launch the EXE directly)
; ============================================================

[Icons]

Name: "{autoprograms}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"; \
    WorkingDir: "{app}"; \
    IconFilename: "{app}\images\logo.ico"

Name: "{autodesktop}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"; \
    WorkingDir: "{app}"; \
    IconFilename: "{app}\images\logo.ico"


; ============================================================
; UNINSTALL
; ============================================================

[UninstallRun]

; Close the application if it is running
Filename: "{sys}\taskkill.exe"; \
    Parameters: "/F /IM {#MyAppExeName}"; \
    Flags: runhidden waituntilterminated; \
    RunOnceId: "KillIPISv2"

; Delete IPISv2 scheduled task
Filename: "{sys}\schtasks.exe"; \
    Parameters: "/Delete /TN ""{#MyTaskName}"" /F"; \
    Flags: runhidden waituntilterminated; \
    RunOnceId: "DeleteIPISv2Task"


; ============================================================
; CODE
; ============================================================

[Code]

{ ------------------------------------------------------------
  Close a running copy so files can be replaced on upgrade
  ------------------------------------------------------------ }
procedure CloseRunningApp();
var
  ResultCode: Integer;
begin
  Exec(ExpandConstant('{sys}\taskkill.exe'),
       '/F /IM {#MyAppExeName}',
       '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;


{ Runs after "Ready to Install", before Setup checks for files in use.
  Closing the app here avoids the "applications are using files"
  prompt on upgrade. }
function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  CloseRunningApp();
  Result := '';
end;


{ ------------------------------------------------------------
  Fallback when PowerShell is blocked: creates the same task
  through the Task Scheduler COM API. Unlike schtasks /Create it
  can set the working directory, no 3-day run limit and the
  battery options.
  ------------------------------------------------------------ }
function CreateStartupTaskCom(ExePath, AppDir: String): Boolean;
var
  Service, Folder, TaskDef, Principal, Settings: Variant;
  Triggers, Trigger, Actions, Action: Variant;
begin
  Result := False;
  try
    Service := CreateOleObject('Schedule.Service');
    Service.Connect();
    Folder := Service.GetFolder('\');
    TaskDef := Service.NewTask(0);

    Principal := TaskDef.Principal;
    Principal.GroupId := 'BUILTIN\Administrators';
    Principal.RunLevel := 1;                      { TASK_RUNLEVEL_HIGHEST }

    Settings := TaskDef.Settings;
    Settings.Enabled := True;
    Settings.StartWhenAvailable := True;
    Settings.DisallowStartIfOnBatteries := False;
    Settings.StopIfGoingOnBatteries := False;
    Settings.ExecutionTimeLimit := 'PT0S';        { no time limit }

    Triggers := TaskDef.Triggers;
    Trigger := Triggers.Create(9);                { TASK_TRIGGER_LOGON }
    Trigger.Delay := 'PT10S';

    Actions := TaskDef.Actions;
    Action := Actions.Create(0);                  { TASK_ACTION_EXEC }
    Action.Path := ExePath;
    Action.WorkingDirectory := AppDir;

    { 6 = TASK_CREATE_OR_UPDATE, 4 = TASK_LOGON_GROUP }
    Folder.RegisterTaskDefinition('{#MyTaskName}', TaskDef, 6,
      'BUILTIN\Administrators', Null, 4);

    Result := True;
    Log('Startup task created through the Task Scheduler COM API.');
  except
    Log('Task Scheduler COM API failed: ' + GetExceptionMessage);
  end;
end;


{ ------------------------------------------------------------
  Creates the logon startup task.

  A PowerShell script is used because it allows setting the
  task's WORKING DIRECTORY (schtasks /Create cannot do this),
  a logon delay, and battery-friendly settings.

  Principal: BUILTIN\Administrators group, Run Level Highest
  -> starts elevated at logon for any administrator account,
     with no UAC prompt.
  ------------------------------------------------------------ }
procedure CreateStartupTask();
var
  ScriptPath: String;
  Script: String;
  ExePath: String;
  AppDir: String;
  ResultCode: Integer;
  Success: Boolean;
begin
  Success := False;
  ResultCode := -1;
  ExePath := ExpandConstant('{app}\{#MyAppExeName}');
  AppDir := ExpandConstant('{app}');
  ScriptPath := ExpandConstant('{tmp}\IPISv2_CreateTask.ps1');

  Script :=
    '$ErrorActionPreference = ''Stop''' + #13#10 +
    '$exe = ''' + ExePath + '''' + #13#10 +
    '$dir = ''' + AppDir + '''' + #13#10 +
    '$action = New-ScheduledTaskAction -Execute $exe -WorkingDirectory $dir' + #13#10 +
    '$trigger = New-ScheduledTaskTrigger -AtLogOn' + #13#10 +
    '$trigger.Delay = ''PT10S''' + #13#10 +
    '$principal = New-ScheduledTaskPrincipal -GroupId ''BUILTIN\Administrators'' -RunLevel Highest' + #13#10 +
    '$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries ' +
      '-StartWhenAvailable -ExecutionTimeLimit ([TimeSpan]::Zero)' + #13#10 +
    'Register-ScheduledTask -TaskName ''{#MyTaskName}'' -Action $action -Trigger $trigger ' +
      '-Principal $principal -Settings $settings -Force | Out-Null' + #13#10;

  if SaveStringToFile(ScriptPath, Script, False) then
  begin
    if Exec(
         ExpandConstant('{sys}\WindowsPowerShell\v1.0\powershell.exe'),
         '-NoProfile -ExecutionPolicy Bypass -File "' + ScriptPath + '"',
         '',
         SW_HIDE,
         ewWaitUntilTerminated,
         ResultCode
       ) and (ResultCode = 0) then
    begin
      Success := True;
      Log('Startup task created with PowerShell.');
    end
    else
      Log('PowerShell task creation failed. Exit code: ' + IntToStr(ResultCode));

    DeleteFile(ScriptPath);
  end
  else
    Log('Unable to write PowerShell script: ' + ScriptPath);

  { A plain schtasks fallback is deliberately not used: Windows gives such
    a task a 3-day run limit and "start only on AC power", which stops a
    24/7 app. The Task Scheduler COM API sets everything correctly. }
  if not Success then
  begin
    Log('Using Task Scheduler COM fallback.');
    if not CreateStartupTaskCom(ExePath, AppDir) then
      Log('Startup task could not be created. IPISv2 will not start at logon.');
  end;
end;


procedure CurStepChanged(CurStep: TSetupStep);
begin
  { Second safety net: the logon task could have restarted the app
    between PrepareToInstall and the file copy. }
  if CurStep = ssInstall then
    CloseRunningApp();

  if CurStep = ssPostInstall then
    CreateStartupTask();
end;
