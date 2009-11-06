"Flash Card" 
An generic flash card application for the Microsoft Surface that is easily customized.

The MIT License

Copyright (c) 2009 William C. Kurt

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.


The "Flash Card" application was designed so that it would be very easy for non-programmers to customize the application.

For this reason the application relies very heavily on naming conventions.

By default all data for the flash cards goes into the "C:\flash_card_data" directory

in that directory should be folders that associate collections of images with byte-tags (which can be associated with real world objects)

the naming convention for those folders is:
<byte-tag number in DECIMAL>-<whatever you want>
so:
00-bodyparts\  should be a set of images associated with the byte tag labeled "00"

remember that the number is in DECIMAL! and the byte-tags are in hex

so:
10-books\ would associate a set of images with the tag "0A"

all of the images must be stores in a \images\ folders

the structure ofeach images is meaing full as well:

<name>-U.<extension> is the Unlabeld image: letterA-U.jpg
<name>-L.<extension> is the Labeled image: letterA-L.jpg

in the directory 'default_data' there is an example zip file.
extract "flash_card_data.zip" to C:\ and the default application should run

Also in the project Resources is a file named 
"svBackground.png" replace this file with another to change the background image
the .exe must be recompiled for any changes to take effect.

please send any questions to wckurt@gmail.com