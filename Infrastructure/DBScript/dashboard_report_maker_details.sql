CREATE OR REPLACE FUNCTION public.dashboard_report_maker_details(
	_user_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select dashboard_report_maker_details(null)
*/
BEGIN
    RETURN (
        SELECT json_agg(report_data)
        FROM (
            SELECT
                aspnet_users.user_id,
                aspnet_users.name,

                -- Active Cases
                COUNT(
                    CASE 
                        WHEN "case".case_status_id NOT IN (5,6,7) 
                        THEN 1 
                    END
                ) AS active_cases

            FROM public.aspnet_users 

            LEFT JOIN public."case" ON "case".report_maker_id = aspnet_users.user_id

            WHERE 
                aspnet_users.is_active = true
                AND (
                    _user_id IS NULL 
                    OR aspnet_users.user_id = _user_id
                )

            GROUP BY aspnet_users.user_id, aspnet_users.name

            ORDER BY active_cases DESC
        ) AS report_data
    );
END;
$BODY$;

ALTER FUNCTION public.dashboard_report_maker_details(_user_id integer)
    OWNER TO postgres;
