CREATE OR REPLACE FUNCTION public.case_status_log_selectall(
	_id integer,
	_case_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select case_status_log_selectall(null::int, null::int);
    select case_status_log_selectall(1::int, null::int);
    select case_status_log_selectall(null::int, 1::int);
*/
BEGIN
    RETURN (
        SELECT
            json_agg(case_status_log)
        FROM
        (
            SELECT
                case_status_log.id,
                case_status_log.case_id,
                case_status_log.case_status_id,
                case_status_mas.name AS case_status,
                case_status_log.remark,
                case_status_log.last_modified_by,
                case_status_log.last_modified_date,
                "case".customer_name,
                "case".customer_contact,
                reporter.name AS report_maker,
                visitor.name AS visitor
            FROM public.case_status_log
            INNER JOIN public.case_status_mas ON case_status_mas.id = case_status_log.case_status_id
            INNER JOIN public."case" ON "case".id = case_status_log.case_id
            LEFT JOIN public.aspnet_users visitor ON "case".visitor_id = visitor.user_id
            LEFT JOIN public.aspnet_users reporter ON "case".report_maker_id = reporter.user_id
            WHERE
                ( _id IS NULL OR case_status_log.id = _id )
                AND ( _case_id IS NULL OR case_status_log.case_id = _case_id )
            ORDER BY
                case_status_log.last_modified_date DESC
        ) AS case_status_log
    );
END

$BODY$;

ALTER FUNCTION public.case_status_log_selectall(_id integer, _case_id integer)
    OWNER TO postgres;
