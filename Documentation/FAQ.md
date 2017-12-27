## FAQ:
### When the program starts updating the ddns?
On first open: after you fill and confirm settings;  
Otherwise as soon as you open it.

### Where settings are saved?
In a text file named "dduSettings.txt" located where the program is stored  

### What is the file format?
Currently is a simple text file, for more info check [file format specifications](File%20format%20specifications.md).

### Is it portable?
Yes, if you copy the program and optionally the settings file to another pc it will work (if .net framework is installed)

### Why you use .net framework i don't like it.
That's not a question; anyway i use it because it's a safe language (check [security FAQ](Security%20FAQ.md) for more details).

### Can i set it to automatically start with windows?
Yes, you set it to run with windows and you can forget about always opening it; very useful.  
It will start in background so there will not be a window.  
If for some reason you want to stop it you can terminate it's process or disable autorun and reboot.

### Why there are two autorun options?
In this way you can run the program only after your user login and you don't need admin privileges to run it (you may don't trust it enough to give it admin privileges). 
If you want to start it on boot also if there is no user logged you must run it as admin; this option is grey (disabled) if you are not admin.  
If you are interested in how it works:  
-user autorun uses a regedit key hkcu\...\run  
-admin/boot autorun uses task scheduler (and makes a copy in program files to avoid privilege escalation bug [like this](https://www.exploit-db.com/exploits/9305/))

### What is the log option and what does it log?
I respect your privacy and i don't collect anything from you!! NEVER!  
Log option can be activated via GUI or by command line.  
`-LogTemp` creates a log file in %TEMP%\ddu.log where %temp% is a variable that point to _current_ user temp directory (so if you auto run on boot probably c:\windows\temp, dont get tricked).  
`-LogHere` creates a log file where the program is stored.  
`-NoLog` disable logging.  
Log options given from command line are not saved and do not persist if you reopen the program, while if you set them from gui they are saved and used also if you open the program in background mode.  
You should not set more than one log option, if you do, only the first option received will be used  
Inside the file you can find useful informations about the update result history (aka if the program is working correctly).  
After the update an entry like this should be logged: `Updating DNS: OK`  
Note that background mode never exit, so ensure that you have a working configuration before using it.  
possible vlaues are:  

|Reported status| Reason|Will stop updater in GUI mode
|---|---|---
|OK|DDNS updated succesfully (doesn't matter if the ip changed or not)|no
|NotConnected| you are not connected, there is a nework problem, a firewall is blocking access|no
|Firewalled|resolved dns update link to ip but can't connect to that ip, usually firewall but can be network problem or disconnected.|no
|UserNotFound|connected to the ddns provider but the server report(404) that user doesn't exist|yes
|Unauthorized|connected to the ddns provider but the server report(401) that password (or user) is wrong|yes
|UpdateFailed|connected to the ddns provider but the server returned an unexpected answer or no answer at all|yes
|InvalidUpdateLink|Bad link format / not a link|yes
|UnknownError|Unknown error, check the log for more informations|yes
### Why the password is stored in plaintext in settings file?
Because is the safest way and there is no other way than this; check the [security FAQ](Security%20FAQ.md) for more deails.

### Is there a linux version?
No, since linux has ddclient (which i have never tested).