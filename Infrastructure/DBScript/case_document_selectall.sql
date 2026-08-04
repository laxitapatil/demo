CREATE OR REPLACE FUNCTION public.case_document_selectall(
	_id integer,
	_property_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select case_document_selectall(null, 14)
*/
BEGIN
    RETURN (
        SELECT JSON_BUILD_OBJECT(

            'documents', JSON_BUILD_OBJECT(

                -- Bank Documents (doc_type = 1)
                'bank_documents', (
                    SELECT JSON_AGG(JSON_BUILD_OBJECT(
                        'id', case_document.id,
                        'title', case_document.title,
                        'doc_type', case_document.doc_type,
                        'file_path', case_document.file_path,
                        'created_by', case_document.created_by,
                        'created_date', case_document.created_date,
                        'modified_by', case_document.modified_by,
                        'modified_date', case_document.modified_date
                    ))
                    FROM case_document
                    WHERE (_id IS NULL OR case_document.id = _id)
                      AND (_property_id IS NULL OR case_document.property_id = _property_id)
                      AND case_document.doc_type = 1
                ),

                -- Property Documents (doc_type = 2)
                'property_documents', (
                    SELECT JSON_AGG(JSON_BUILD_OBJECT(
                        'id', case_document.id,
                        'title', case_document.title,
                        'doc_type', case_document.doc_type,
                        'file_path', case_document.file_path,
                        'created_by', case_document.created_by,
                        'created_date', case_document.created_date,
                        'modified_by', case_document.modified_by,
                        'modified_date', case_document.modified_date
                    ))
                    FROM case_document
                    WHERE (_id IS NULL OR case_document.id = _id)
                      AND (_property_id IS NULL OR case_document.property_id = _property_id)
                      AND case_document.doc_type = 2
                ),

                -- Visit Sheet (doc_type = 3)
                'visit_sheet', (
                    SELECT JSON_AGG(JSON_BUILD_OBJECT(
                        'id', case_document.id,
                        'title', case_document.title,
                        'doc_type', case_document.doc_type,
                        'file_path', case_document.file_path,
                        'created_by', case_document.created_by,
                        'created_date', case_document.created_date,
                        'modified_by', case_document.modified_by,
                        'modified_date', case_document.modified_date
                    ))
                    FROM case_document
                    WHERE (_id IS NULL OR case_document.id = _id)
                      AND (_property_id IS NULL OR case_document.property_id = _property_id)
                      AND case_document.doc_type = 3
                ),

                -- Case Documents (doc_type = 4)
                'case_documents', (
                    SELECT JSON_AGG(JSON_BUILD_OBJECT(
                        'id', case_document.id,
                        'title', case_document.title,
                        'doc_type', case_document.doc_type,
                        'file_path', case_document.file_path,
                        'created_by', case_document.created_by,
                        'created_date', case_document.created_date,
                        'modified_by', case_document.modified_by,
                        'modified_date', case_document.modified_date
                    ))
                    FROM case_document
                    WHERE (_id IS NULL OR case_document.id = _id)
                      AND (_property_id IS NULL OR case_document.property_id = _property_id)
                      AND case_document.doc_type = 4
                )

            )

        )
    );
END;

$BODY$;

ALTER FUNCTION public.case_document_selectall(_id integer, _property_id integer)
    OWNER TO postgres;
