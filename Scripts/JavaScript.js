<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
@section Scripts {
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        $(document).ready(function () {
            var currentDate = new Date();
        var formattedDate = currentDate.toISOString().substring(0, 19).replace("T", " ");
        $("#action_date").val(formattedDate);
        });
    </script>
}
