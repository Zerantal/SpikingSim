Release notes for version 0.33 of the mathml dom and mathml renderer.

An update to version 0.32. News here is the full and correct 
implementation of the action element, and lots of new events.

News with the 0.32 release is element selection is now working
and enabled. Just double click on an element area with the mouse
and it will become selected. An 'AfterSelect' event will be fired
by the control.

Experimental mathml editing is also included. This is disabled
by default as it currently does not work very well, to enable
editing, set the 'ReadOnly' property to false. Just select an
element with the mouse to position the cursor. Move the cursor 
with the arrow keys, and start typing to insert items. The 
delete key will delete the item currently after the cursor. 

NOTE, editing is VERY EXPERIMENTAL, it WILL CRASH!!! Do not 
enable editing in any sort of released application.

This control is work in progress, so  there are
bound to be some problems. PLEASE let me know of ANY problems, 
and let me known if you have ANY questions. send mail to
andy@epsilon3.net

Requirements:
All componenents require version 1.1 of the .net common language runtime, 
availible at: http://microsoft.com/downloads/details.aspx?FamilyId=262D25E3-F589-4842-8157-034D1E7CF3A3&displaylang=en

File List:

- MathML.dll
A nearly complete and faithfull implementation of the full
w3c published mathml dom specification. All wc3 published 
interfaces are implemented, and most methods are implemented
and functional. This is used very extensivly by the 
mathml rendering component. Note, that the use of the mathml 
dtd is NOT required. All mathml entities are resolved internally.
If your source mathml document does contain a reference to a 
mathml dtd, that dtd does however need to be in the same
directory, or else the xml parser will complain. You can however
just create a blank file with the proper dtd name, and all should 
work.

- MathML.Rendering.dll
A WinForms control that renders a mathml document. Currently 
most presention elements are working, but no content markup
elements will be rendered. In the future, this controll will
allow editing, and will respond to dynamic documents. In order 
to use this component in your own application, your application
needs to have a config file with a "mathml-rendering-config", 
and this entry needs to have a value that specifies the location
of the font config files. You can have the MS Visual Studio do
this automatically for you by adding a App.config file to 
your exe project. You can just edit this fill and add the
entry for the font config files, To see an example, take a look
at the MathML.Rendering.Test1.exe.config file.


- MathML.Rendering.Test1.exe 
A test application that hosts the MathML.Rendering control, and
allows the user to open and display mathml documents.

- gnumerator.chm
Full documentation of every single class used in these components.

- font-configuration-*.xml 
font config files that the rendering control uses to map
glyphs and fonts.

- MathML.Rendering.Test1.exe.config
the config file for the application. this currently contains
a single entry for the directory of the font config files. 
just open this file up with notepad, and set the value
of the "mathml-rendering-config" to the directory where
you placed the font config files.

- math-roman.ttf and cm-stretchy.ttf
These are standard true type fonts, created from the AMS tex fonts, 
with STANDARD unicode encoding of most mathematical characters. The 
original glyphs were pulled from the AMS tex fonts. These fonts are 
required by the rendering component.
