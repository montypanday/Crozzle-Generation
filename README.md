# Crozzle-Generation
This is the crozzle generation Windows Forms Project which was made as a part of a project. The goal is to achieve expected scores. Heuristic and Depth First algorithm is used for making crozzle from three different set of files. The scoring scheme, available words, and size is different for the all the three sets. The files are present in the repository. It is to be noted that the algorithm works for the given set of files. This project aims to generate high scoring crozzle with use of valid words and configurations. It may fail with incorrect files and may not generate high scores with all different configurations and marking schemes possible.


Section A
This section demonstrates the working of application for Marking Test 1. Please note that the algorithm is random and will generate different score every time.

 
Figure 1 Marking Test 1 Crozzle


// File dependencies.

// Configuration file.

CONFIGURATION_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.cfg"


// Word list file. 

WORDLIST_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.wls"


// Crozzle Size.

// The number of rows and columns.

ROWS=10

COLUMNS=15


// Word Data.

// The horizontal rows containing words.

ROW=7,ZUZANNY,9

ROW=8,ZILI,1

ROW=3,LIZZIE,10

ROW=6,ROZY,3

ROW=1,OZZIE,9

ROW=4,ZEUS,4

ROW=8,LIZ,7

ROW=10,ED,10

ROW=9,YEE,13

ROW=10,YASMIN,3

ROW=1,ERICA,1

ROW=3,LU,3

ROW=2,LEO,6


// The vertical rows containing words.

COLUMN=9,KIZZY,5

COLUMN=11,EZZARD,5

COLUMN=13,ZUZANNY,3

COLUMN=1,KIZZIE,5

COLUMN=3,RYLEY,6

COLUMN=10,ZILI,1

COLUMN=5,ENZO,4

COLUMN=15,AMY,5

COLUMN=15,LE,2

COLUMN=4,CRUZ,1

COLUMN=6,LOU,2

COLUMN=7,AL,7

COLUMN=1,EVA,1


Section B

This section demonstrates the working of application for Marking Test 2. This Crozzle is formed using depth first build 

algorithm using a tree.

 
Figure 2 Marking Test 2 Crozzle

// File dependencies.

// Configuration file.

CONFIGURATION_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.cfg"


// Word list file. 

WORDLIST_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.wls"


// Crozzle Size.

// The number of rows and columns.

ROWS=12

COLUMNS=18


// Word Data.

// The horizontal rows containing words.

ROW=9,OZZIE,14

ROW=6,DEZZIE,13

ROW=3,ZUZANNY,11

ROW=1,ZACK,10

ROW=11,EZZARD,11

ROW=8,ZACHARY,6

ROW=11,ZACHERY,2

ROW=5,HEZZIE,2

ROW=1,JAZZLYN,2

ROW=9,ZORBA,1

ROW=7,CHAZ,2

ROW=6,ROZY,8

ROW=4,LIZ,8


// The vertical rows containing words.

COLUMN=17,KIZZIE,5

COLUMN=14,ZAVIER,2

COLUMN=12,CRUZ,1

COLUMN=16,ZENA,1

COLUMN=14,OMAR,9

COLUMN=11,RYLEY,8

COLUMN=7,ZAKARY,7

COLUMN=3,ZECHARIAH,4

COLUMN=6,LIZZIE,1

COLUMN=9,TIMOTHY,3

COLUMN=3,AL,1


Section C

This section demonstrates the application making a Crozzle for Marking Test 3. It uses random generation algorithm; thus, it 

will make different Crozzle every time.


Figure 3 Marking Test 3 Crozzle


// File dependencies.

// Configuration file.

CONFIGURATION_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.cfg"


// Word list file. 

WORDLIST_FILE="http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.wls"


// Crozzle Size.

// The number of rows and columns.

ROWS=8

COLUMNS=12


// Word Data.

// The horizontal rows containing words.

ROW=1,AYLA,3

ROW=3,ROZY,4

ROW=5,CRUZ,5

ROW=4,JACK,1

ROW=2,TAI,7

ROW=4,IAN,8

ROW=6,AB,8

ROW=7,ED,1

ROW=6,LE,3

ROW=1,VAL,9

ROW=3,ABE,10

ROW=5,ACE,10

ROW=7,EDDY,9

ROW=8,ALAN,6

ROW=7,LU,4


// The vertical rows containing words.

COLUMN=4,YORK,1

COLUMN=6,ZARA,3

COLUMN=7,TY,2

COLUMN=8,IZA,4

COLUMN=1,JAKE,4

COLUMN=2,BEA,2

COLUMN=3,CAL,4

COLUMN=9,VI,1

COLUMN=10,ANA,3

COLUMN=9,BEN,6

COLUMN=4,ELA,6

COLUMN=7,AL,7


Appendix

The files used for generation of this algorithm can be found at following addresses.

Appendix A

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.czl

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.cfg

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.wls


Appendix B

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.czl

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.cfg

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.wls


Appendix C

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.czl

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.cfg

http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.wls
