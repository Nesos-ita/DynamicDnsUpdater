## Security FAQ:
### Is this program secure? Why?
* It's secure to the best of my knowledge and it has been written with security in mind (what does it mean? nothing!)  
* It is signed so you can check that you have a program that is written by me and that you have an original copy of it that has not been modified by bad things in the middle [(HAPPENS!!!!)](https://www.eff.org/deeplinks/2014/11/starttls-downgrade-attacks).  
In this way you can limit the trust that you have to put to: me, my pc and programs that i use; you don't need to care about GitHub or any evil router in the middle while downloading it.  
* It's written in a safe language (C# / .NET) so many bad things can't happen (like accessing or writing outside the limits of an array aka buffer overflow)  
more info here: <https://www.classle.net/#!/classle/content-page/net-safe-language/>  
and here: <http://big.info/2014/01/what-does-type-safe-mean-c.html>  
* It uses DEP (Data Execution Prevention)  
* It uses ASLR (Address Space Layout Randomization)  
* It uses ASLR in every dll that it uses (which is NOT a secondary thing [look what can happen if you don't use aslr in every module](https://www.exploit-db.com/exploits/36207/))  
(keep in mind that having enabled only one between dep or aslr doesn't help at all and if aslr is not used in every single dll it is useless)  
* It places dobulequota when setting autorun if there is a space in the path (try to copy some program in C:\ and call it program.exe, weird things might happen...)
* It enforce tls1.2 or 1.3, but note that windows does not support tls1.3 yet so we will have to wait that they add in schannel support for tls1.3 (will probably happen in the future automatically with window update) 

### How do i know that the exe doesn't have a backdoor that is not present in the source code?
You can compile it yourself if you don't trust the exe and then [check the differences](https://madiba.encs.concordia.ca/%7Ex_decarn/truecrypt-binaries-analysis/) 
or you can decompile it with [ILSpy](http://ilspy.net/) and read the the decompiled result yourself.

### Why password is stored in plaintext? isn't that a bad idea? why don't you encrypt it?
Short answer: because to encrypt a password i need another passowrd; doesn't solve the problem.  
Why don't you obfuscate it? because it only gives a false sense of security and project is open source.  
For more details keep reading:  
there are two kind of stored paswords:  
-passwords stored where you have control (on your pc)  
-passwords stored out of your control (internet)  
The first type (like the windows login, full disk encryption password, password manager _master_ pasword) can be stored in a secure way: by not storing it at all; this might seems weird but is the only secure way to do it.  
What must be proved is that the user knows the correct password and can be done without knowing the password: in fact what is stored is a (stalted) hash of the password (or nothing at all for encryption passwords).  
The second type is the facebook login, email login, desec password...  
_they_ store a (salted) hash of the password because they own the service but we must send them a plaintext password (encrypted _while in transit_ using https)  
so it doesn't matter if i encode/obfuscate/encrypt with aes256/...; right before i send it to them it must be plaintext and if someone want to steal it he have only to wait that the program decrypt it for him.  
Think like the copy protection of music it is stupid and useless, as soon as you can listen it, you can record it (the analog hole) and since music is designed to be listened there is no hope. Storing the password is somehow a similar problem.  
The only secure way to store a password as i said is not store it at all but this is not an option because we want that this program works automatically.  
Please keep in mind that for example viewing saved passowrds of wifi or browser takes less than one second, there is nothing to crack; the fact that you don't know where/how they are stored doesn't make them more secure; it only gives you a false sense of security.  
If your pc is not full disk encrypted and is used by someone that you don't trust (probably thee is no hope but...) edit the acl on the file and deny access to anyone except you (and deny the ability to change acl too).  

### How to verify the digital signature?
You will need [GPG (Gnu Privacy Guard)](https://gnupg.org/) or [Gpg4win](https://www.gpg4win.org/).  
You will also need to read it's manual.  
As i said above [Qubes OS](https://www.qubes-os.org/doc/verifying-signatures/) has a detailed page on digital signatures and what they can (and can't) prove.  
Basic steps are:  
* Obtaining a copy of my public key and importing it (you need to do this only _once_)  
* Verifying that the obtained key is authentic (if you trust git https it is)  
* Download program and it's signature  
* Verify the signature  

### Why the key is not on keyservers?
Because keyservers are just a place to download keys and the key can be downloaded directly from here, from a security point of view they are useless (they are not used to prove anything).  
