; Inno Setup Script for Tic Tac Toe
; This script installs the application at the user level without requiring admin permissions.

[Setup]
; Basic Setup Information
AppName=Tic Tac Toe
AppVersion=1.0.0
DefaultDirName={localappdata}\Tic Tac Toe
DefaultGroupName=Tic Tac Toe
OutputBaseFilename=TicTacToe_v1.0.0_x64_Setup
OutputDir=.
PrivilegesRequired=lowest
Compression=lzma2
SolidCompression=yes
UninstallDisplayIcon={app}\tictactoe.ico
SetupIconFile=..\tictactoe.ico
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
; UserInfoPage=yes uncommnet it to make it work!
[Files]
; Application Files
Source: "..\bin\Release\net8.0-windows\publish\win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\tictactoe.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Desktop Shortcut
Name: "{userdesktop}\Tic Tac Toe"; Filename: "{app}\Tic Tac Toe.exe"; IconFilename: "{app}\tictactoe.ico"

; Start Menu Shortcut
Name: "{group}\Tic Tac Toe"; Filename: "{app}\Tic Tac Toe.exe"; IconFilename: "{app}\tictactoe.ico"

[Run]
; Optional: Run the application after installation
Filename: "{app}\Tic Tac Toe.exe"; Description: "Launch Tic Tac Toe"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Optional: Clean up additional files if needed
Type: files; Name: "{app}\*"
Type: dirifempty; Name: "{app}"

[Code]
// Optional: Custom Pascal Script code can be added here if needed
