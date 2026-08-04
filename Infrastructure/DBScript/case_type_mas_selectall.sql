CREATE OR REPLACE FUNCTION public.case_type_mas_selectall(
	_id smallint,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select case_type_mas_selectall(null::smallint, null::BOOLEAN)
*/
BEGIN
	RETURN (
		SELECT
			JSON_AGG ( case_type_mas )
		FROM ( SELECT
				case_type_mas.id,
			 	case_type_mas.name,
			 	case_type_mas.is_active,
                case_type_mas.modified_by,
                case_type_mas.modified_date
			FROM public.case_type_mas
			WHERE
			  	( _id IS NULL OR case_type_mas.id = _id )
			  	AND ( _is_active IS NULL OR case_type_mas.is_active = _is_active )
			ORDER BY
			  	case_type_mas.name ASC 
    ) AS case_type_mas );
END
$BODY$;

ALTER FUNCTION public.case_type_mas_selectall(_id smallint, _is_active boolean)
    OWNER TO postgres;
