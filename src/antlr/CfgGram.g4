grammar CfgGram;


gram: 'grammar' grammar_name ';'
       ('lang' grammar_lang ';')?
       imports?
       rules ;
grammar_name:  STARTS_BIG;
grammar_lang: ALL_SMALL;

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
// Импорты грамматик и нетерминалов
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////

imports: (imprt)+;
imprt: grammar_import | nonterm_import;
grammar_import:  'import'
        imp_src_grammar_name
        ('as' imp_local_grammar_name)?
        EXPRESSION_END
        ;

imp_src_grammar_name: STARTS_BIG;
imp_local_grammar_name: STARTS_BIG;

nonterm_import:
        'from'
        imp_src_grammar_name
        'import'
        imp_nonterm_elem
        (
            ','
            imp_nonterm_elem
        )*
        ';'
        ;
imp_nonterm_elem: imp_src_nonterminal_name ('as' imp_local_nonterminal_name)? ;
imp_src_nonterminal_name: STARTS_BIG;
imp_local_nonterminal_name: STARTS_BIG;

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
// Правила
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////

rules: (rule ';')+;

rule : rule_name
       rule_template?
       '->'
       rule_extension_query?
       (
            rule_expression
            |
            rule_expression_list
       );

rule_name : STARTS_BIG;
rule_expression: rule_item+
                 rule_static_vars?;
rule_expression_list: rule_expression ('|' rule_expression)+;

rule_extension_query: '<'rule_extention_type rule_extention_query_text;
rule_extention_query_text: EXTENTION_VALUE;
rule_extention_type: ALL_SMALL;



rule_item :
        rule_item_head_flag?
        rule_item_main_key
        rule_item_counter?
        rule_item_conditions?
        ('as' rule_item_local_name)?
        ;

rule_item_head_flag: '!';

rule_item_main_key: rule_inline_expression | rule_item_static_main_key;
rule_inline_expression : '(' rule_expression ')';
rule_item_static_main_key:
        rule_item_static_main_key_lemma |
        rule_item_static_main_key_reg |
        rule_item_static_main_key_morph |
        rule_item_static_main_key_string |
        rule_item_static_main_key_short_nonterm |
        rule_item_static_main_key_full_nonterm |
        rule_item_variable_name;
rule_item_static_main_key_lemma: LEMMA_STRING_VALUE;
rule_item_static_main_key_reg: REG_STRING_VALUE;
rule_item_static_main_key_morph: MORPH_STRING_VALUE;
rule_item_static_main_key_string: STRING_VALUE;
rule_item_static_main_key_short_nonterm: STARTS_BIG;
rule_item_static_main_key_full_nonterm: STARTS_BIG '.' STARTS_BIG;
rule_item_variable_name: '$' ALL_SMALL;







rule_item_counter: rule_item_plus_counter | rule_item_star_counter | rule_item_question_counter | rule_item_number_counter;
rule_item_plus_counter: '+';
rule_item_star_counter: '*' ;
rule_item_question_counter: '?';
rule_item_number_counter: '{' rule_item_number_counter_min ',' rule_item_number_counter_max '}';
rule_item_number_counter_min: NUMBER*;
rule_item_number_counter_max: NUMBER+ ;
rule_item_local_name:  ALL_SMALL;


rule_item_conditions : '<' rule_item_condition (';' rule_item_condition)*  '>';
rule_item_condition :  rule_item_condition_negation? (rule_item_condition_flag | rule_item_condition_key_pair);
rule_item_condition_negation: '~';
rule_item_condition_flag: (ALL_SMALL | CYRILLIC_ALL_SMALLL);
rule_item_condition_key_pair: rule_item_condition_key '=' (rule_item_condition_value | rule_item_condition_value_list);
rule_item_condition_key: ALL_SMALL | CYRILLIC_ALL_SMALLL;
rule_item_condition_value_list: rule_item_condition_value (',' rule_item_condition_value)*;
rule_item_condition_value:  rule_item_condition_value_literal | rule_item_condition_value_string;
rule_item_condition_value_literal: ALL_SMALL |  NUMBER | CYRILLIC_ALL_SMALLL;
rule_item_condition_value_string: string_type_value;


rule_static_vars:   rule_static_var+ ;
rule_static_var:  '#set' rule_static_var_name '=' rule_static_var_value;
rule_static_var_name: ALL_SMALL;
rule_static_var_value:
     rule_static_string_value |
     rule_static_integer_value |
     rule_static_float_value |
     rule_static_bool_value ;
rule_static_string_value: string_type_value;
rule_static_integer_value: integer_type_value;
rule_static_float_value: float_type_value;
rule_static_bool_value: bool_type_value;



// Шаблоны извлечения
rule_template: '[' rule_template_member ( ',' rule_template_member)* ']';
rule_template_member: rule_template_member_key '=' rule_template_member_value;

rule_template_member_key: STARTS_BIG;
rule_template_member_value: rule_template_value;
rule_template_value: rule_template_value_list  |
                 rule_template_value_name_reference |
                 rule_template_value_number_reference |
                 rule_template_value_string |
                 rule_template_value_integer |
                 rule_template_value_float |
                 rule_template_value_bool
                 ;
rule_template_value_list:  '{' rule_template_value (',' rule_template_value)+ '}';
rule_template_value_name_reference: '$' rule_template_value_name_reference_key ('.' rule_template_value_name_reference_value)?;
rule_template_value_name_reference_key: ALL_SMALL;
rule_template_value_name_reference_value: STARTS_BIG;
rule_template_value_number_reference: '$' rule_template_value_number_reference_key ('.' rule_template_value_number_reference_value)?;
rule_template_value_number_reference_key:  NUMBER ;
rule_template_value_number_reference_value: STARTS_BIG;
rule_template_value_string: string_type_value;
rule_template_value_integer: integer_type_value;
rule_template_value_float: float_type_value;
rule_template_value_bool: bool_type_value;


















string_type_value: STRING_VALUE;
integer_type_value: '-'? NUMBER;
float_type_value: '-'?  NUMBER '.' NUMBER;
bool_type_value: 'true' | 'false';

///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////

EXPRESSION_END: ';';
ONTOLOGY_SEPARATOR_PREFIX: '.';
NUMBER: [0-9]+;
STARTS_BIG: [A-Z][a-zA-Z_0-9]*;
CYRILLIC_STARTS_BIG: [А-Я][а-яА-Я0-9_]*;
ALL_SMALL: [a-z][a-z_0-9]*;
CYRILLIC_ALL_SMALLL: [а-я][а-я0-9_]*;
REG_STRING_VALUE : 'r' '"' (~[\r\n\f] )+  '"';
LEMMA_STRING_VALUE : 'l' '"' (~[\\\r\n\f] )+  '"';
MORPH_STRING_VALUE : 'm' '"' (~[\\\r\n\f] )+  '"';
STRING_VALUE : '"' (~[\r\n\f"] )*  '"';
EXTENTION_VALUE : '>' .*?  '</end>';
LIST_POSTFIX: '[]';
WS : [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines
COMMENT
    : '/*' .*? '*/' -> skip
;

LINE_COMMENT
    : '//' ~[\r\n]* -> skip
;
