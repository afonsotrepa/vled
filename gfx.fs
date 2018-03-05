#! /usr/bin/gforth

s" ncurses" add-lib

\c #include <ncurses.h>
\c #define printcr() printw("\n");
\c #define keypadw() keypad(stdscr, TRUE);

c-function initscr initscr -- void
c-function raw raw -- void
c-function noecho noecho -- void
c-function echo echo -- void
c-function keypadw keypadw -- void
c-function refresh refresh -- void
c-function clearw clear -- void
c-function endwin endwin -- void

c-function getch getch -- n
c-function printcr printcr -- void
c-function printw printw a -- void
c-function movew move n n -- void
c-function addchw addch n -- void

10 constant enter-key
27 constant esc-key
261 constant right-key
260 constant left-key
259 constant up-key
258 constant down-key
263 constant backspace-key
330 constant delete-key

variable cursor-x 0 cursor-x !
variable cursor-y 0 cursor-y !
