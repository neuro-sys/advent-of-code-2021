: load-data ( addr u -- ) create included ;

: reduce ( xt addr n -- acc ) ( xt: addr acc -- addr acc )
  >r
  begin
    dup @ 0<>
  while
    over r> swap execute >r
    cell +
  repeat
  2drop r>
;

: for-each ( xt addr -- ) ( xt: addr -- addr )
  begin
    dup @ 0<>
  while
    over execute
    cell +
  repeat
  2drop
;

: +for-each ( xt addr n -- ) ( xt: addr -- addr )
  >r
  begin
    dup @ 0<>
  while
    over execute
    r> dup >r cells +
  repeat
  2drop rdrop
;

create "data 10000 cells allot
variable "dp "data "dp !

: "allot ( n -- )    "dp +! ;
: "here  ( -- addr ) "dp @ ;

\ Save ccc in string buffer as counted string and leave address
: ," ( "ccc" -- addr ) [char] " parse   ( caddr u -- )
     dup "here c!                       \ save count
     "here >r                           ( caddr u -- ) ( R: addr )
     1 "allot                           \ skip 1 byte
     "here swap dup >r cmove r> "allot  \ copy string and skip u bytes
     r>                                 \ restore addr
;


: ```forth ;
: ``` begin refill tib 8 s" ```forth" compare 0 = until ;
: ```ignore begin refill tib 3 s" ```" compare 0 = until ;
: ... ``` ;
