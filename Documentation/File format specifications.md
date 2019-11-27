# File format specification
The file format is currently a simple text file with an option per line.  

    user
    password
    hostname
    update url
    update delay
    log mode
    update also on local IP change
You should use the gui to edit all settings and, if possible, avoid editing the file directly.  
All options are mandatory.  
The file format might change in future versions.  

**User:** username  
**Password:** password  
**Hostname:** hostname  
**Update url:** must be only the domain (sub domain included if necesary), the full update url will be: `https://updateurl?hostname=hostname` as you can see in gui.  
**Update delay:** a number expressed in minutes between 1min and 24h (greater values are truncated to the max allowed)  
**Log mode:** one of the following strings are allowed: `noLog`, `logHere`, `logTemp`.  
**Update also on local IP change:** one of the following strings are allowed: `True`, `False`  