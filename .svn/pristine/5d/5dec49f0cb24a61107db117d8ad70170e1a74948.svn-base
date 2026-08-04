(function ($) {

    $.fn.uploadBox = function (options) {

        const settings = $.extend({
            icons: {
                pdf: "/assets/images/icons/pdf.png",
                doc: "/assets/images/icons/doc.png",
                file: "/assets/images/icons/file.png"
            }
        }, options);

        function getPreview(file) {
            if (file.type.startsWith("image/")) {
                return { type: "image", src: null };
            }
            if (file.type === "application/pdf") {
                return { type: "icon", src: settings.icons.pdf };
            } 
            if (
                file.type === "application/msword" ||
                file.type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            ) {
                return { type: "icon", src: settings.icons.doc };
            }
            return { type: "icon", src: settings.icons.file };
        }

        function singleTemplate(name) {
            return `
                <input type="file" name="${name}" class="upload-input d-none">

                <div class="upload-ui" style="height:250px; cursor:pointer;">
                    <div class="upload-placeholder text-center">
                        <i class="fi fi-rr-cloud-upload"></i><br>
                        Upload File
                    </div>
                    <div class="upload-preview">
                        <img class="preview-img d-none">
                    </div>
                </div>

                <div class="upload-actions d-none text-center">
                    <button type="button" class="btn btn-primary btn-sm edit-upload">
                        <i class="fi fi-rr-pencil"></i>
                    </button>
                    <button type="button" class="btn btn-danger btn-sm remove-upload">
                        <i class="fi fi-rr-trash"></i>
                    </button>
                </div>
            `;
        }

        function multipleTemplate(name, src) {
            return `
                <div class="col-lg-3 col-md-6 upload-box">
                    <input type="file" name="${name}" class="upload-input d-none">

                    <div class="upload-ui">
                        <div class="upload-preview">
                            <img src="${src}" class="preview-img">
                        </div>
                    </div>

                    <div class="upload-actions text-center">
                        <button type="button" class="btn btn-primary btn-sm edit-upload">
                            <i class="fi fi-rr-pencil"></i>
                        </button>
                        <button type="button" class="btn btn-danger btn-sm remove-upload">
                            <i class="fi fi-rr-trash"></i>
                        </button>
                    </div>
                </div>
            `;
        }

        /* ---------------- INIT SINGLE ---------------- */
        this.each(function () {
            const $box = $(this);
            const accept = $box.data("accept") || "*/*";
            const name = $box.data("name") || "file";

            $box.addClass("upload-box");
            $box.html(singleTemplate(name));

            $box.on("click", ".upload-ui", function () {
                $box.find(".upload-input").attr("accept", accept).trigger("click");
            });

            $box.on("click", ".edit-upload", function (e) {
                e.preventDefault();
                e.stopPropagation();
                $box.find(".upload-input").trigger("click");
            });

            $box.on("change", ".upload-input", function () {
                const file = this.files[0];
                if (!file) return;

                const preview = getPreview(file);
                const $img = $box.find(".preview-img");

                if (preview.type === "image") {
                    const reader = new FileReader();
                    reader.onload = e => $img.attr("src", e.target.result).removeClass("d-none");
                    reader.readAsDataURL(file);
                } else {
                    $img.attr("src", preview.src).removeClass("d-none");
                }

                $box.find(".upload-placeholder").hide();
                $box.find(".upload-actions").removeClass("d-none");
            });

            $box.on("click", ".remove-upload", function () {
                $box.find(".upload-input").val("");
                $box.find(".preview-img").attr("src", "").addClass("d-none");
                $box.find(".upload-placeholder").show();
                $box.find(".upload-actions").addClass("d-none");
            });
        });

        /* ---------------- MULTIPLE ---------------- */
        $(document).on("click", "[data-upload-trigger]", function () {

            // find the upload-multiple related to THIS button only
            const $container = $(this)
                .closest(".card")          // scope to section
                .find(".upload-multiple")  // get its container
                .first();

            if (!$container.length) return;

            const accept = $container.data("accept") || "*/*";
            const name = $container.data("name");

            const $picker = $(`<input type="file" multiple accept="${accept}" hidden>`);
            $("body").append($picker);

            $picker.on("change", function () {

                Array.from(this.files).forEach(file => {

                    const preview = getPreview(file);
                    const reader = new FileReader();

                    reader.onload = function (e) {
                        const src = preview.type === "image" ? e.target.result : preview.src;
                        const $el = $(multipleTemplate(name, src));

                        const dt = new DataTransfer();
                        dt.items.add(file);
                        $el.find(".upload-input")[0].files = dt.files;

                        $container.append($el);
                    };

                    reader.readAsDataURL(file);
                });

                $picker.remove();
            });

            $picker.trigger("click");
        });

        $(document).on("click", ".upload-multiple .edit-upload", function () {
            $(this).closest(".upload-box").find(".upload-input").trigger("click");
        });

        $(document).on("change", ".upload-multiple .upload-input", function () {
            const file = this.files[0];
            if (!file) return;

            const $box = $(this).closest(".upload-box");
            const preview = getPreview(file);
            const $img = $box.find(".preview-img");

            if (preview.type === "image") {
                const reader = new FileReader();
                reader.onload = e => $img.attr("src", e.target.result);
                reader.readAsDataURL(file);
            } else {
                $img.attr("src", preview.src);
            }
        });

        $(document).on("click", ".upload-multiple .remove-upload", function () {
            $(this).closest(".upload-box").remove();
        });

        return this;
    };

})(jQuery);
