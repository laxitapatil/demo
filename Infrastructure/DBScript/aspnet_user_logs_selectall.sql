CREATE OR REPLACE FUNCTION public.aspnet_user_logs_selectall(
	_from_date timestamp without time zone,
	_to_date timestamp without time zone,
	_search_text character varying)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
   Example:
   SELECT aspnet_user_logs_selectall('2026-01-01','2026-01-31', 'prachi');
   SELECT aspnet_user_logs_selectall(null, null, 'prachi');
*/
BEGIN
    RETURN (
        SELECT
            JSON_AGG( user_logs )
        FROM (
            SELECT
                aspnet_user_logs.id,
                aspnet_user_logs.user_name,
                aspnet_users.name AS name,
                aspnet_user_logs.access_type,
                aspnet_user_logs.location,
                aspnet_user_logs.task_category,
                aspnet_user_logs.status_code,
                aspnet_user_logs.message,
                aspnet_user_logs.created_date
            FROM public.aspnet_user_logs
            LEFT JOIN public.aspnet_users ON aspnet_users.username = aspnet_user_logs.user_name
            WHERE
                (_from_date IS NULL OR aspnet_user_logs.created_date >= _from_date)
                AND (_to_date IS NULL OR aspnet_user_logs.created_date < (_to_date + INTERVAL '1 day'))
                AND (
                    _search_text IS NULL OR _search_text = '' OR
                    LOWER(aspnet_user_logs.user_name) LIKE '%' || LOWER(_search_text) || '%' OR
                    LOWER(aspnet_users.name) LIKE '%' || LOWER(_search_text) || '%' OR
                    LOWER(aspnet_user_logs.task_category) LIKE '%' || LOWER(_search_text) || '%'
                )
            ORDER BY aspnet_user_logs.created_date DESC
        ) AS user_logs
    );
END;
$BODY$;

ALTER FUNCTION public.aspnet_user_logs_selectall(_from_date timestamp without time zone, _to_date timestamp without time zone, _search_text character varying)
    OWNER TO postgres;
