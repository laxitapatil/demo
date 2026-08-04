CREATE OR REPLACE FUNCTION public.aspnet_users_selectall(
	_id character varying,
	_user_id integer,
	_role character varying,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select aspnet_users_selectall('34287ea9-553e-4e02-ab6e-5922022b1dd2',null,null,null)
*/
BEGIN
	RETURN (
		SELECT
			JSON_AGG ( aspnet_users )
		FROM ( SELECT
				aspnet_users.id,
			 	aspnet_users.username,
			 	aspnet_users.email,
			 	aspnet_users.phone,
			 	aspnet_users.user_id,
			 	aspnet_users.name,
			 	aspnet_users.is_active,
                aspnet_users.profile_image,
			  	aspnet_roles.name AS role,
                aspnet_roles.id AS role_id
			FROM
				public.aspnet_users
			  	LEFT JOIN public.aspnet_user_roles ON aspnet_user_roles.user_id = aspnet_users.id
			  	LEFT JOIN public.aspnet_roles ON aspnet_roles.id = aspnet_user_roles.role_id
			WHERE
			  	( _id IS NULL OR aspnet_users.id = _id )
			  	AND ( _user_id IS NULL OR aspnet_users.user_id = _user_id )
			  	AND ( _role IS NULL
					 OR ( ( LOWER ( _role ) IN ( 'admin', 'staff', 'manager' ) AND LOWER ( aspnet_roles.name ) IN ( 'admin', 'staff', 'manager' ) )
						 OR LOWER ( aspnet_roles.name ) = LOWER ( _role ) )
					)
			  	AND ( _is_active IS NULL OR aspnet_users.is_active = _is_active )
			ORDER BY
				aspnet_users.username ) AS aspnet_users );
END
$BODY$;

ALTER FUNCTION public.aspnet_users_selectall(_id character varying, _user_id integer, _role character varying, _is_active boolean)
    OWNER TO postgres;
