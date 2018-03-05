#! /usr/bin/gforth
\ Visual Line EDitor

require gfx.fs
require cons.fs

4096 constant max-length
4096 constant max-lines 
variable lines   nil lines ! \ list holding the lines
variable #lines  0 #lines !
: inc 	 ( addr --)  1 swap +! ;
: dec 	 ( addr --)  -1 swap +! ;
: x      cursor-x @ ;
: y      cursor-y @ ;
: l      y lines @ nth ;

variable fh  nil fh ! \ file handler
2variable file \ name of the file

: len    ( str -- u)  0   begin dup >r   over + c@ 0 <>   while r> 1+ repeat 
	 drop r> ;
: strcpy ( str1 -- str2)  dup len 1+ dup >r   here swap move   here   r> allot ;

: open 	 ( addr u fam --)  open-file throw fh ! ; 
: close  ( --)  fh @ close-file throw ;
: read   ( addr u --)  r/o open   begin pad max-length fh @ read-line throw while
	 dup dup   1+ allocate throw tuck   pad swap rot cmove   tuck + 0 swap c!
	 lines @ nil <> if lines @ lappend else nil cons lines ! then
	 #lines inc   repeat   drop  close ;
: write  ( addr u --)  w/o open   lines @ begin dup   car@ dup len fh @
	 write-line  throw   dup cdr@  dup nil = until  drop ;

: update ( --)  refresh   y x movew ;
: print	 ( --)  clearw   lines @ begin dup nil <> while dup car@ printw printcr
	 cdr@ repeat   drop   update ;

: down   ( --)  cursor-y #lines < if cursor-y inc update then ;
: up 	 ( --)  cursor-y dec update ;
: right	 ( --)  cursor-x inc update ;
: left 	 ( --)  cursor-x dec update ;

: rep 	 ( c --)  l  x + c! ;
: replace ( c --)   rep   cursor-x inc  update ;

: nl 	 ( --)  cursor-y @ lines @ nthcdr   dup car@   dup x + strcpy
	 rot dup rot swap cdr@ cons swap cdr!   0 rep
	 cursor-y inc   0 cursor-x !   #lines inc   print ;

: ins 	 ( c a1 a2 i--)  >r   2dup i cmove   i + 1+  swap i + swap  dup >r
	 over len 1+  cmove  r>   1- c!   rdrop ;
: insert ( c--)  cursor-y @ lines @ nthcdr dup >r car@ dup >r  
	 dup len 2 + allocate throw dup >r x ins
	 r>   r> free throw   r> car!   cursor-x inc update ;

: del 	 ( n a--)  +   dup 1+   tuck len 1+   cmove ;
: join   ( --)  
: delete ( --)  x l 2dup len < if del else 2drop join then print ;

: init 	 ( --)  initscr raw noecho keypadw   next-arg file 2!   file 2@ read
	 print ;
: handle ( --)  getch case 
	 down-key      of down endof 
	 up-key        of up endof 
	 right-key     of right endof
	 left-key      of left endof
	 esc-key       of endwin   file 2@ write   bye endof
	 enter-key     of nl endof
	 backspace-key of endof
	 delete-key    of delete endof
	 dup insert print  endcase ;

: main   ( --)  init begin handle again ;

main
