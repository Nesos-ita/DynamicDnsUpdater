# DynamicDnsUpdater
***Allows you to keep updated the Dynamic DNS easily***  
![screenshot](Documentation/screenshot.png)  
## How to use and configure:  
1.  Download the program under [Compiled Binary/DynamicDnsUpdater.exe](https://github.com/Nesos-ita/DynamicDnsUpdater/raw/master/Compiled%20Binary/DynamicDnsUpdater.exe)  
2. Optional but suggested: Check the digital signature using [my public key](Documentation/Nesos%20code%20signing%20public%20key.asc). (You can find more informations about digital signatures and why they are important [here](https://www.qubes-os.org/doc/verifying-signatures/))  
3. Open and follow on screen instructions  (fill data and click ok)

## For [deSEC](https://desec.io/#!/en/) users:  
if you received an email from deSEC about dropping support to tls1.0 i have released a new version that enforce tls1.2 and support tls1.3 ensure to have the latest version.  
- username field: fill with your hostname (example: something.dedyn.io)  
- password field: fill with your password (you should have received it by email)  
- hostname field: fill with your hostname (same as username field)  
- update url: is yet correct, no need to edit  
- delay: i think that every 60 minutes is a good option (delay between 1 min - 24 hours)  

Click OK to confirm and that is all what you need to do!
## Documentation
If for some reason you don't have .NET framework 4.8 you should be able to download it from here: [.NET 4.8 (64bit)](http://go.microsoft.com/fwlink/?LinkId=2085155)  

If there is an error different from a connection problem, for example: wrong pasword, the updater will stop (red square); Check if settings are correct.  
If something doesn't work try to enable and view the log, it has useful informations inside; you can also set the log option from commandline.  

Let me know if you find any bug by reporting them  
Also report feature requests  

For more complete and detailed informations check:  
**[FAQ](Documentation/FAQ.md)**  
**[Security FAQ](Documentation/Security%20FAQ.md)**  
**[File format specifications](Documentation/File%20format%20specifications.md)**  
**[Version history](Documentation/Version%20history.md)**  
