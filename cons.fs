#! /usr/bin/gforth

: cons   ( car cdr -- cons)  2 cells allocate throw   tuck cell + !  dup -rot ! ;
: car@ 	 ( cons -- n)  @ ;
: cdr@ 	 ( cons -- n)  cell + @ ;
: car!   ( n cons --)  ! ;
: cdr!   ( n cons --)  cell + ! ;


\ functions to work on lists
: lappend  ( n list --)  begin  dup cdr@ nil <> while cdr@  repeat
	   swap nil cons   swap cdr! ;
: nthcdr   ( u list -- cons) swap dup 0 > if 0 do cdr@ loop else drop then ;
: nth 	   ( u list -- n)  nthcdr car@ ;
: length   ( list -- u) recursive
	   dup nil =   if 0 drop else cdr@ length 1+ then ;
