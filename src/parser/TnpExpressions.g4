grammar TnpExpressions ;

tnp_expression: expr ;

expr:
	STRING_LITERAL # String
	| BOOLEAN_LITERAL # Boolean
	| number # Digits
	| qualified_identifier # QualifiedIdentifier
	| expr '[' expr ']' # Array
	| '(' expr ')' # Parens
	| prefix=('+'|'-'|'~'|'!') expr # Prefix
	| expr binaryop=('*'|'/'|'%') expr # Mult
	| expr binaryop=('+'|'-') expr # Add
	| expr ('<' '<' | '>' '>') expr # Shift
	| expr binaryop=('<=' | '>=' | '>' | '<') expr # Compare
	| expr binaryop=('==' | '!=') expr # Equals
	| expr binaryop='&' expr # BitAnd
	| expr binaryop='^' expr # Xor
	| expr binaryop='|' expr # BitOr
	| expr binaryop='&&' expr # And
	| expr binaryop='||' expr # Or
	| <assoc=right> expr binaryop='?' expr ':' expr # Ternary
	| expr '(' ')' # CallSimple
	| expr '(' parameterList ')' # CallParams
	;

parameterList:
	parameter (',' parameter)*
	;

parameter: expr
	| 'out' identifier
	| 'ref' identifier
	;

number: DECIMAL_LITERAL | HEX_LITERAL | FLOAT_LITERAL ;

qualified_identifier: identifier (identifier '.')* ;

identifier: IDENTIFIER;

BOOLEAN_LITERAL:
	'true'
	| 'false'
	;

STRING_LITERAL:	'"' (~["\\\r\n] | EscapeSequence)* '"';

DECIMAL_LITERAL:	('0' | [1-9] (Digits? | '_'+ Digits)) [lL]?;
HEX_LITERAL:		'0' [xX] [0-9a-fA-F] ([0-9a-fA-F_]* [0-9a-fA-F])? [lL]?;
FLOAT_LITERAL:		(Digits '.' Digits? | '.' Digits) ExponentPart? [fFdD]?
	| 		Digits (ExponentPart [fFdD]? | [fFdD])
	;

IDENTIFIER:	Letter LetterOrDigit*;

fragment LetterOrDigit:
	Letter
	| [0-9]
	;

fragment Letter:
	[a-zA-Z_]
	;

fragment EscapeSequence: '\\' 'u005c'? [btnfr"'\\]
	| '\\' 'u005c'? ([0-3]? [0-7])? [0-7]
	| '\\' 'u'+ HexDigit HexDigit HexDigit HexDigit
	;

fragment HexDigit: [0-9a-fA-F]
	;

fragment Digits: [0-9] ([0-9_]* [0-9])?
	;

fragment ExponentPart:
	[eE] [+-]? Digits
	;
