s" lib.fs" included
s" 2.txt"  load-data data

2 constant data-width  variable position   variable depth
                       variable aim

: forward ( n -- ) dup position +! aim @ * depth +! ;
: down    ( n -- ) aim +! ;
: up      ( n -- ) aim @ swap - aim ! ;

: command ( addr -- caddr n ) @ count ;
: amount        ( addr -- n ) dup cell + @ ;

: command ( addr -- addr )
  dup amount swap command evaluate ;

: process ['] command data data-width +for-each ;
: solve   process position @ depth @ * ;

solve . bye
