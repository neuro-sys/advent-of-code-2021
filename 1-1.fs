s" lib.fs" included
s" 1.txt"  load-data data

: depth<   dup cell - @   over @  < ;
: collect  swap depth< if 1 else 0 then rot + ;
: solve    ['] collect data cell + 0 reduce ;

solve .

bye
