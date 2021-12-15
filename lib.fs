[undefined] lib.fs [if]
create lib.fs

0 warnings !

\ Create an array with name ccc, of n elements, of u size
\ First cell contains the count
\ Second cell contains the size of element
\ "3 cell array ccc" creates ccc with 3 elements of cell size
\ 0 ccc returns first element
\ -2 ccc returns the count
\ -1 ccc returns the element size
: array ( n u "<spaces>ccc<space>" -- )
  create
  swap                    ( u n )
  here 2 allot swap       ( u addr n )
  dup cells allot         ( u addr n )
  over !                  ( u addr )
  cell + !                ( )

  does> dup cell + @      ( n addr u )
  rot 2 + * +             ( addr )
;

: array-load ( caddr u addr -- )
  dp !
  included
;

: #array ( "<spaces>ccc<space" -- n )
  0
  32 word find 0= throw execute 2 cells - @
  state @ if [compile] literal then
; immediate

: mark        ( -- addr ) here 0 , ;
: count! ( addr -- addr ) here over - cell / 1- over ! cell + ;

: reduce ( xt addr n acc -- acc ) ( xt: addr acc -- addr acc )
  >r               ( xt addr ) ( R: acc )
  >r               ( xt addr ) ( R: acc n )
  begin
    r@ 0<>
  while
    over           ( xt addr xt ) ( R: acc n )
    2r> >r         ( xt addr xt acc ) ( R: n )
    swap           ( xt addr acc xt ) ( R: n )
    execute        ( xt addr acc ) ( R: n )
    r> 2>r         ( xt addr ) ( R: acc n )
    r> 1- >r       ( xt addr ) ( R: acc n )
    cell +         ( xt addr ) ( R: acc n )
  repeat
  2drop 2r> drop
;

: for-each ( xt addr n -- ) ( xt: addr -- addr )
  >r               ( xt addr ) ( R: n )
  begin
    r@ 0<>
  while
    over execute
    cell +
    r> 1- >r
  repeat
  2drop rdrop
;

: binary 2 base ! ;

: #count ( addr -- n )
  0 swap
  begin
    dup @ 
  while
    cell + swap 1+ swap
  repeat drop
;


: assert ( n -- )
  cr 2dup <> if
    ." Test failed: " . ." is not equal " .
  else
    ." Test passed" 2drop
  then
;

\ String words
10000 constant #"heap

create "data #"heap cells allot
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

\ Markdown words
: `skip ( caddr u -- )
  begin
    2dup refill 0=
    if 2drop 2drop exit then
    tib over 2swap compare 0 =
  until 2drop
;

: ```       s" ```forth" `skip ;
: ```forth ;
: ... ``` ;

\ use this to print out the forth code
: md-code
  begin
    refill
  while
    ```
    refill drop
    cr tib #tib @ type
  repeat
  bye
;

\ file loading and parsing
variable fd
variable n

1000 constant #buf
create buf #buf allot

: open-file  ( caddr u -- ) r/o open-file throw fd ! ;
: read-line ( -- u ) buf #buf fd @ read-line throw swap n ! ;

: /blank ( caddr u -- caddr u )
  begin
    over c@ 32 = >r
    dup r> and
  while
    1 /string
  repeat
;

: parse-number ( -- )
  buf n @
  begin
    dup
  while
    /blank
    2dup s"  " search if
      dup >r 2swap r> - evaluate ,
    else
      evaluate , 2drop exit
    then
    /blank
  repeat 2drop
;

: parse-matrix ( m -- )
  0 do
    read-line 0= if drop -1 unloop exit then parse-number
  loop 0
;

: parse-matrix-all ( n -- ) begin dup parse-matrix until ;

[then]
