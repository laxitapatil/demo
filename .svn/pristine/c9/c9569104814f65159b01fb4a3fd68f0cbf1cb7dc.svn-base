CREATE OR REPLACE FUNCTION public.dashboard_case_statistics(
	_visitor_id integer,
	_reporter_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select dashboard_case_statistics(null,null)
*/
BEGIN
    RETURN (
        SELECT jsonb_build_object(

            'new_cases',
            COUNT(*) FILTER (
                WHERE case_status_id = 1
            ),

            'site_visit_cases',
            COUNT(*) FILTER (
                WHERE case_status_id = 2
            ),

            'report_in_progress_cases',
            COUNT(*) FILTER (
                WHERE case_status_id = 3
            ),

            'under_review_cases',
            COUNT(*) FILTER (
                WHERE case_status_id = 4
            ),

            'completed_cases',
            COUNT(*) FILTER (
                WHERE case_status_id = 5
            ),

            'hold_cancelled_cases',
            COUNT(*) FILTER (
                WHERE case_status_id IN (6,7)
            )

        )
        FROM public."case"
        WHERE
        (
            (_visitor_id IS NOT NULL AND "case".visitor_id = _visitor_id)
            OR
            (_reporter_id IS NOT NULL AND "case".report_maker_id = _reporter_id)
            OR
            (_visitor_id IS NULL AND _reporter_id IS NULL)
        )
    );
END;
$BODY$;

ALTER FUNCTION public.dashboard_case_statistics(_visitor_id integer, _reporter_id integer)
    OWNER TO postgres;
