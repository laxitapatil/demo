CREATE OR REPLACE FUNCTION public.case_status_mas_selectall(
	_id smallint,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select case_status_mas_selectall(null, null);
    select case_status_mas_selectall(1::smallint, null::int);
    select case_status_mas_selectall(null::smallint, 5::int);
*/
BEGIN
    RETURN (
        SELECT
            json_agg(case_status_mas)
        FROM
        (
            SELECT
                case_status_mas.id,
                case_status_mas.name,
                case_status_mas.is_active,
                case_status_mas.modified_by,
                case_status_mas.modified_date
            FROM
                public.case_status_mas 
            WHERE
                ( _id IS NULL OR case_status_mas.id = _id )
                AND ( _is_active IS NULL OR case_status_mas.is_active = _is_active )
            ORDER BY
                case_status_mas.id
        ) AS case_status_mas
    );
END
$BODY$;

ALTER FUNCTION public.case_status_mas_selectall(_id smallint, _is_active boolean)
    OWNER TO postgres;
