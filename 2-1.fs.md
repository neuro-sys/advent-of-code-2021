s" lib.fs" included
s" 2.txt"  load-data data

2 constant data-width  variable position   variable depth

: forward ( n -- ) position +! ;
: down    ( n -- ) depth +! ;
: up      ( n -- ) depth @ swap - depth ! ;

: command ( addr -- caddr n ) @ count ;
: amount        ( addr -- n ) dup cell + @ ;

: command ( addr -- addr )
  dup amount swap command evaluate ;

: process ['] command data data-width +for-each ;
: solve   process position @ depth @ * ;

solve . bye
