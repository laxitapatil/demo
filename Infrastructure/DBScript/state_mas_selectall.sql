CREATE OR REPLACE FUNCTION public.state_mas_selectall(
	_id integer,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select state_mas_selectall(null::integer, null::BOOLEAN)
*/
BEGIN
	RETURN (
		SELECT
			JSON_AGG ( state_mas )
		FROM ( SELECT
				state_mas.id,
			 	state_mas.name,
			 	state_mas.is_active,
                state_mas.last_modified_by,
                state_mas.last_modified_date
			FROM public.state_mas
			WHERE
			  	( _id IS NULL OR state_mas.id = _id )
			  	AND ( _is_active IS NULL OR state_mas.is_active = _is_active )
			ORDER BY
			  	state_mas.name ASC 
    ) AS state_mas );
END
$BODY$;

ALTER FUNCTION public.state_mas_selectall(_id integer, _is_active boolean)
    OWNER TO postgres;
