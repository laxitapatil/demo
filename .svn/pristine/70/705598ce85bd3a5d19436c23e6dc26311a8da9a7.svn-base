CREATE OR REPLACE FUNCTION public.dashboard_last_updated_cases(
	_limit integer,
	_visitor_id integer,
	_reporter_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select dashboard_last_updated_cases(5, null,null)
*/
DECLARE
    v_limit integer := COALESCE(_limit, 10);
BEGIN
    RETURN (
        SELECT json_agg(case_data)
        FROM (
            SELECT
                "case".id,
                "case".case_id,
                "case".created_date,
                "case".customer_name,
                bank.name AS bank,
                bank.code,
                case_status_mas.name AS case_status,
                visitor.name AS visitor,
                report_maker.name AS report_maker,

                "case".priority AS priority_id,

                CASE 
                    WHEN "case".priority = 1 THEN 'High'
                    WHEN "case".priority = 2 THEN 'Medium'
                    WHEN "case".priority = 3 THEN 'Low'
                END AS priority,

                --  total property count
                (
                    SELECT COUNT(cp.id)
                    FROM public.case_property cp
                    WHERE cp.case_id = "case".id
                ) AS property_count,

                -- case_status_log.last_modified_date
                "case".modified_date
            FROM public."case"

            LEFT JOIN public.bank ON "case".bank_id = bank.id

            LEFT JOIN public.case_status_mas  ON "case".case_status_id = case_status_mas.id

            LEFT JOIN public.aspnet_users visitor  ON "case".visitor_id = visitor.user_id

            LEFT JOIN public.aspnet_users report_maker ON "case".report_maker_id = report_maker.user_id

            -- LEFT JOIN LATERAL (
            --     SELECT last_modified_date
            --     FROM public.case_status_log
            --     WHERE case_id = "case".id
            --     ORDER BY last_modified_date DESC
            --     LIMIT 1
            -- ) case_status_log ON TRUE

            -- ORDER BY COALESCE(case_status_log.last_modified_date, "case".created_date) DESC
            WHERE
            (
                (_visitor_id IS NOT NULL AND "case".visitor_id = _visitor_id)
                OR
                (_reporter_id IS NOT NULL AND "case".report_maker_id = _reporter_id)
                OR
                (_visitor_id IS NULL AND _reporter_id IS NULL)
            )
            ORDER BY 
            COALESCE("case".modified_date, "case".created_date) DESC

            LIMIT v_limit
        ) AS case_data
    );
END;
$BODY$;

ALTER FUNCTION public.dashboard_last_updated_cases(_limit integer, _visitor_id integer, _reporter_id integer)
    OWNER TO postgres;
