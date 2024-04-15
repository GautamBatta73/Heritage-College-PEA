<?php

declare(strict_types=1);
error_reporting(E_ALL);
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
header("Content-Security-Policy: frame-ancestors 'none'; script-src 'self';");
header('Access-Control-Allow-Origin: null;');
session_start();

include './functions/leader-functions.php';
include './functions/player-functions.php';
require './classes/class-leader.php';
require './classes/class-player.php';
require './enums/enum-country.php';
$player_list = null;
if (isset($_SESSION["email"])) {
    $player_list = set_players($_SESSION["email"]);
}
?>

<?php
//Form Validation
$action_done = false;

$error = false;
$email = "";
$password = "";
$conf_password = "";

$display_form = true;
$first_name = "";
$last_name = "";
$nick_name = "";
$id = "";
$city = "";
$country = "";
$is_professional = false;
$img = "";

if (isset($_POST["btnSubmitLogin"])) {
    if (!isset($_SESSION["logged_in"])) {
        $email = isset($_POST["fldEmail"]) ? trim($_POST["fldEmail"]) : "";
        $password = isset($_POST["fldPassword"]) ? trim($_POST["fldPassword"]) : "";
        if (valid_login($email, $password)[0]) {
            $_SESSION["logged_in"] = serialize(get_leader($email)) ?? serialize(new Leader());
            $_SESSION["email"] = $email;
            $_SESSION["login_time"] = date_format(new DateTime("now", new DateTimeZone("America/Toronto")), 'Y-m-d H:i:s');
            $player_list = set_players($email);
            $error = false;
        } else {
            $error = true;
            if (isset($_SESSION["logged_in"])) {
                unset($_SESSION["logged_in"]);
                unset($_SESSION["email"]);
                unset($_SESSION["login_time"]);
                $player_list = null;
            }
            $error = true;
        }
    }
    unset($_POST["btnSubmitLogin"]);
    $action_done = true;
} else if (isset($_POST["btnSubmitAdd"]) || isset($_POST["btnSubmitEdit"])) {
    $first_name = isset($_POST["fldFName"]) ? trim($_POST["fldFName"]) : "";
    $last_name = isset($_POST["fldLName"]) ? trim($_POST["fldLName"]) : "";
    $nick_name = isset($_POST["fldNName"]) ? trim($_POST["fldNName"]) : "";
    $id = isset($_POST["fldId"]) ? trim($_POST["fldId"]) : "-1";
    $city = isset($_POST["fldCity"]) ? trim($_POST["fldCity"]) : "";
    $country = isset($_POST["fldCountry"]) ? trim($_POST["fldCountry"]) : "";
    $is_professional = isset($_POST["fldProf"]);
    $img = isset($_FILES["fldImg"]["name"]) ? ($_FILES["fldImg"]["name"] ?? "") : "";

    $error = (
        (!check_name($first_name)[0]) ||
        (!check_name($last_name)[0]) ||
        (str_contains($nick_name, "~")) ||
        (!check_id($id)[0]) ||
        (!check_city($city)[0]) ||
        (!check_country($country)[0]) ||
        (!check_image($img)[0])
    );
    if (!$error) {
        $player = new Player($first_name, $last_name, $nick_name, (int)$id, $city, Country::from_val($country), $is_professional, null);
        $error = !add_player($player, $_SESSION["email"]);
    }
    $action_done = true;
    $display_form = $error;
} else if (isset($_POST["btnSubmitAddLeader"])) {
    $first_name = isset($_POST["fldFName"]) ? trim($_POST["fldFName"]) : "";
    $last_name = isset($_POST["fldLName"]) ? trim($_POST["fldLName"]) : "";
    $email = isset($_POST["fldEmail"]) ? trim($_POST["fldEmail"]) : "";
    $password = isset($_POST["fldPassword"]) ? trim($_POST["fldPassword"]) : "";
    $conf_password = isset($_POST["fldConfirmPassword"]) ? trim($_POST["fldConfirmPassword"]) : "";

    $error = (
        (!check_name($first_name)[0]) ||
        (!check_name($last_name)[0]) ||
        (!check_email($email)[0]) ||
        (!valid_pass($password)[0])
    );

    if (!$error) {
        $error = !confirm_password($conf_password, password_hash($password, PASSWORD_DEFAULT))[0];

        if (!$error) {
            $leader = new Leader($email, password_hash($password, PASSWORD_DEFAULT), $first_name, $last_name);
            add_leader($leader);
        }
    }
    $action_done = true;
    $display_form = $error;
}


if (isset($_GET["action"]) && !$action_done) {
    if (isset($_SESSION["logged_in"]) && isset($_SESSION["email"])) {
        if ($_GET["action"] === 'edit') {
            if (is_numeric($_GET["id"])) {
                $player_list = set_players($_SESSION["email"]) ?? [];
                $foundIndex = get_player_index($player_list, $_GET["id"], $_SESSION["email"]);
                if ($foundIndex > -1) {
                    $foundPlayer = array_filter($player_list, fn ($player) => $player->get_id() == (int)$_GET["id"]);
                    $player = $foundPlayer[0];
                    $first_name = $player->get_first_name();
                    $last_name = $player->get_last_name();
                    $nick_name = $player->get_nick_name();
                    $id = $player->get_id() . "";
                    $city = $player->get_city();
                    $country = $player->get_country()->value;
                    $is_professional = $player->get_is_professional();
                    $img = $player->get_img();
                    delete_player($id, $_SESSION["email"]);
                } else {
                    header("Location: player-league.php");
                }
            } else {
                header("Location: player-league.php");
            }
        } else if ($_GET["action"] === 'del') {
            $id = isset($_GET["id"]) ? $_GET["id"] : "-1";
            if (is_numeric($id)) {
                $foundPlayer = array_filter($player_list, fn ($player) => $player->get_id() == (int)$id);
                if (count($foundPlayer) > 0) {
                    delete_player($id . "", $_SESSION["email"]);
                    $player_list = set_players($_SESSION["email"]);
                }
            }
            header("Location: player-league.php");
        } else if ($_GET["action"] === 'add') {
            $first_name = "";
            $last_name = "";
            $nick_name = "";
            $id = "";
            $city = "";
            $country = "";
            $is_professional = false;
            $img = "";
        } else if ($_GET["action"] === 'logout') {
			session_destroy();
			header("Location: player-league.php");
		}
    } else if ($_GET["action"] === 'addLeader') {
        $first_name = isset($_POST["fldFName"]) ? trim($_POST["fldFName"]) : "";
        $last_name = isset($_POST["fldLName"]) ? trim($_POST["fldLName"]) : "";
        $email = isset($_POST["fldEmail"]) ? trim($_POST["fldEmail"]) : "";
        $password = isset($_POST["fldPassword"]) ? trim($_POST["fldPassword"]) : "";
        $conf_password = isset($_POST["fldConfirmPassword"]) ? trim($_POST["fldConfirmPassword"]) : "";
    } else {
        unset($_GET["action"]);
        if (isset($_GET["id"])) {
            unset($_GET["id"]);
        }
        header("Location: player-league.php");
    }
}
?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=2.0, user-scalable=yes">
    <link href="styles/site.css" rel="stylesheet" type="text/css">
    <link rel="icon" href="images/logo.png" type="image/x-icon">
    <meta charset="utf-8">
    <title>Player League</title>
</head>

<body>
    <?php if (!isset($_SESSION["logged_in"])) : ?>
        <?php if (!isset($_GET["action"])) : ?>
            <h1>Login to Player League</h1>
            <form id="loginForm" method="post">
                <section>
                    <label for="fldEmail">Email: </label>
                    <input name="fldEmail" id="fldEmail" type="text" value="<?= htmlspecialchars($email) ?>">
                </section>

                <section>
                    <label for="fldPassword">Password: </label>
                    <input name="fldPassword" id="fldPassword" type="password">
                </section>

                <?php if ($error && valid_login($email, $password)[0]) : ?>
                    <span class="error">
                        <?= valid_login($email, $password)[1] ?>
                    </span>
                <?php endif ?>

                <input name="btnSubmitLogin" id="btnSubmit" type="submit">
                <span>Don't have an account? <a href="?action=addLeader">Create One!</a></span>
            </form>
        <?php elseif ($_GET["action"] === "addLeader") : ?>
            <?php if ($display_form) : ?>
                <h1>Create New Leader Account</h1>
                <a href="./player-league.php" class="btnBack">Back to Login</a>
                <form id="newLeaderrForm" method="post">
                    <section>
                        <label for="fldFName">First Name: </label>
                        <input name="fldFName" id="fldFName" type="text" value="<?= htmlspecialchars($first_name) ?>" <?= ($error && !check_name($first_name)[0]) ? 'class="error"' : "" ?>>
                        <?php if ($error && !check_name($first_name)[0]) : ?>
                            <span class="error">
                                <?= check_name($first_name)[1] ?>
                            </span>
                        <?php endif ?>
                    </section>

                    <section>
                        <label for="fldLName">Last Name: </label>
                        <input name="fldLName" id="fldLName" type="text" value="<?= htmlspecialchars($last_name) ?>" <?= ($error && !check_name($last_name)[0]) ? 'class="error"' : "" ?>>
                        <?php if ($error && !check_name($last_name)[0]) : ?>
                            <span class="error">
                                <?= check_name($last_name)[1] ?>
                            </span>
                        <?php endif ?>
                    </section>

                    <section>
                        <label for="fldEmail">Email: </label>
                        <input name="fldEmail" id="fldEmail" type="text" value="<?= htmlspecialchars($email) ?>" <?= ($error && !check_email($email)[0]) ? 'class="error"' : "" ?>>
                        <?php if ($error && !check_email($email)[0]) : ?>
                            <span class="error">
                                <?= check_email($email)[1] ?>
                            </span>
                        <?php endif ?>
                    </section>

                    <section>
                        <label for="fldPassword">Password: </label>
                        <input name="fldPassword" id="fldPassword" type="password" <?= ($error && !valid_pass($password)[0]) ? 'class="error"' : "" ?>>
                        <?php if ($error && !valid_pass($password)[0]) : ?>
                            <span class="error">
                                <?= valid_pass($password)[1] ?>
                            </span>
                        <?php endif ?>
                    </section>

                    <section>
                        <label for="fldConfirmPassword">Confirm Password: </label>
                        <input name="fldConfirmPassword" id="fldConfirmPassword" type="password" <?= ($error && !confirm_password($conf_password, $password)[0] && valid_pass($password)[0]) ? 'class="error"' : "" ?>>
                        <?php if ($error && !confirm_password($conf_password, $password)[0] && valid_pass($password)[0]) : ?>
                            <span class="error">
                                <?= confirm_password($conf_password, $password)[1] ?>
                            </span>
                        <?php endif ?>
                    </section>

                    <input name="btnSubmitAddLeader" id="btnSubmit" type="submit">
                </form>
                <?php if ($error && (
                    (check_name($first_name)[0]) &&
                    (check_name($last_name)[0]) &&
                    (valid_pass($password)[0]) &&
                    (confirm_password($conf_password, $password)[0])
                )) : ?>
                    <span class="error">
                        An unexpected error occured.<br>
                        Please try again or come back later.
                    </span>
                <?php endif ?>
            <?php else : ?>
                <h1>Leader Added!</h1>
                <?php unset($_GET["action"]); ?>
                <a href="./player-league.php" class="btnBack">Back to Login</a>
                <h3><?= "$first_name $last_name was successfully added as a leader." ?></h3>
            <?php endif ?>
        <?php endif; ?>
    <?php else : ?>
        <?php if (!isset($_GET["action"])) : ?>
            <h1>Players</h1>
            <div id="leaderInfo">
                <?php $leader_obj = unserialize($_SESSION["logged_in"]) ?>
                <p><span class="boldSpan">Name: </span> <?= htmlspecialchars($leader_obj->get_first_name() . " " . $leader_obj->get_last_name()) ?></p>
                <p><span class="boldSpan">Email: </span><?= htmlspecialchars($leader_obj->get_email()) ?></p>
                <p><span class="boldSpan">Logged In On: </span><?= isset($_SESSION["login_time"]) ? htmlspecialchars($_SESSION["login_time"]) : htmlspecialchars(date_format(new DateTime("now", new DateTimeZone("America/Toronto")), 'Y-m-d H:i:s')) ?></p>
                <a href="?action=logout" id="btnLogout">Logout?</a>
            </div>
            <a href="?action=add" id="btnAddPlayer">Add a Player?</a>
            <div id="playerDiv">
                <?php if ($player_list !== null) : ?>
                    <?php foreach ($player_list as $player) : ?>
                        <section id="player<?= $player->get_id() ?>">
                            <h3>
                                <?= htmlspecialchars($player->get_first_name()) . " " . htmlspecialchars($player->get_last_name()) .
                                    (empty($player->get_nick_name()) ? "" : " (" . htmlspecialchars($player->get_nick_name()) . ")")
                                ?>
                            </h3>
                            <picture>
                                <source srcset="<?= $player->get_img() ?? '' ?>">
                                <img src="./player-images/default.png" alt="No Image Found">
                            </picture>
                            <div>
                                <p><span class="boldSpan">City, Country: </span><?= htmlspecialchars($player->get_city()) . ", " . htmlspecialchars($player->get_country()->value) ?></p>
                                <p><span class="boldSpan">Professional?: </span><?= (($player->get_is_professional()) ? "Yes" : "No") ?></p>
                            </div>
                            <div class="btnDiv">
                                <a href="?action=edit&id=<?= $player->get_id() ?>">Edit</a>
                                <a href="?action=del&id=<?= $player->get_id() ?>">Delete</a>
                            </div>
                        </section>
                    <?php endforeach; ?>
                <?php else : ?>
                    <h3>None to Show!</h3>
                <?php endif; ?>
            </div>
        <?php else : ?>
            <?php if ($_GET["action"] === "add") : ?>
                <?php if ($display_form) : ?>
                    <h1>Add a New Player</h1>
                    <a href="./player-league.php" class="btnBack">Back to Player List</a>
                    <form id="newPlayerForm" method="post" enctype="multipart/form-data">
                        <section>
                            <label for="fldFName">First Name: </label>
                            <input name="fldFName" id="fldFName" type="text" value="<?= htmlspecialchars($first_name) ?>" <?= ($error && !check_name($first_name)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_name($first_name)[0]) : ?>
                                <span class="error">
                                    <?= check_name($first_name)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldLName">Last Name: </label>
                            <input name="fldLName" id="fldLName" type="text" value="<?= htmlspecialchars($last_name) ?>" <?= ($error && !check_name($last_name)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_name($last_name)[0]) : ?>
                                <span class="error">
                                    <?= check_name($last_name)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldNName">Nickname: </label>
                            <input name="fldNName" id="fldNName" type="text" value="<?= htmlspecialchars($nick_name) ?>" <?= ($error && str_contains($nick_name, "~")) ? 'class="error"' : "" ?>>
                            <?php if ($error && str_contains($nick_name, "~")) : ?>
                                <span class="error">
                                    Nickname cannot contain the character '~'
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldId">Unique ID: </label>
                            <input name="fldId" id="fldId" type="number" value="<?= htmlspecialchars($id) ?>" <?= ($error && !check_id($id)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_id($id)[0]) : ?>
                                <span class="error">
                                    <?= check_id($id)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldCity">City: </label>
                            <input name="fldCity" id="fldCity" type="text" value="<?= htmlspecialchars($city) ?>" <?= ($error && !check_city($city)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_city($city)[0]) : ?>
                                <span class="error">
                                    <?= check_city($city)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldCountry">Country: </label>
                            <select name="fldCountry" id="fldCountry" <?= ($error && !check_country($country)[0]) ? 'class="error"' : "" ?>>
                                <option value="" selected>-Select a Country-</option>
                                <?php foreach (Country::cases() as $count) : ?>
                                    <?php if ($count !== Country::unselected) : ?>
                                        <option value="<?= $count->value ?>" <?= $country === $count->value ? "selected" : "" ?>><?= $count->value ?></option>
                                    <?php endif; ?>
                                <?php endforeach; ?>
                            </select>
                            <?php if ($error && !check_country($country)[0]) : ?>
                                <span class="error">
                                    <?= check_country($country)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section id="profSection">
                            <label for="fldProf">Professional?&nbsp;</label>
                            <input name="fldProf" id="fldProf" type="checkbox" <?= $is_professional ? "checked" : "" ?>>
                        </section>

                        <section>
                            <label for="fldImg">Image: </label>
                            <input name="fldImg" id="fldImg" type="file" accept="image/*" <?= ($error && !check_image($img)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_image($img)[0]) : ?>
                                <span class="error">
                                    <?= check_image($img)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <input name="btnSubmitAdd" id="btnSubmit" type="submit">
                    </form>
                    <?php if ($error && (
                        (check_name($first_name)[0]) &&
                        (check_name($last_name)[0]) &&
                        (!str_contains($nick_name, "~")) &&
                        (check_city($city)[0]) &&
                        (check_country($country)[0]) &&
                        (check_image($img)[0])
                    )) : ?>
                        <span class="error">
                            An unexpected error occured.<br>
                            Please try again or come back later.
                        </span>
                    <?php endif ?>
                <?php else : ?>
                    <h1>Player Added!</h1>
                    <?php unset($_GET["action"]); ?>
                    <a href="?action=add" id="btnAddPlayer">Add Another?</a>
                    <a href="./player-league.php" class="btnBack">Back to Player List</a>
                    <h3><?= "$first_name $last_name was successfully added to the list." ?></h3>
                <?php endif ?>
            <?php elseif ($_GET["action"] === "edit") : ?>
                <?php if ($display_form) : ?>
                    <h1>Edit an Existing Player</h1>
                    <a href="./player-league.php" class="btnBack">Back to Player List</a>
                    <form id="newPlayerForm" method="post" enctype="multipart/form-data">
                        <section>
                            <label for="fldFName">First Name: </label>
                            <input name="fldFName" id="fldFName" type="text" value="<?= htmlspecialchars($first_name) ?>" <?= ($error && !check_name($first_name)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_name($first_name)[0]) : ?>
                                <span class="error">
                                    <?= check_name($first_name)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldLName">Last Name: </label>
                            <input name="fldLName" id="fldLName" type="text" value="<?= htmlspecialchars($last_name) ?>" <?= ($error && !check_name($last_name)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_name($last_name)[0]) : ?>
                                <span class="error">
                                    <?= check_name($last_name)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldNName">Nickname: </label>
                            <input name="fldNName" id="fldNName" type="text" value="<?= htmlspecialchars($nick_name) ?>" <?= ($error && str_contains($nick_name, "~")) ? 'class="error"' : "" ?>>
                            <?php if ($error && str_contains($nick_name, "~")) : ?>
                                <span class="error">
                                    Nickname cannot contain the character '~'
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldId">Unique ID: </label>
                            <input name="fldId" id="fldId" type="number" value="<?= htmlspecialchars($id) ?>" <?= ($error && !check_id($id)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_id($id)[0]) : ?>
                                <span class="error">
                                    <?= check_id($id)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldCity">City: </label>
                            <input name="fldCity" id="fldCity" type="text" value="<?= htmlspecialchars($city) ?>" <?= ($error && !check_city($city)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_city($city)[0]) : ?>
                                <span class="error">
                                    <?= check_city($city)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section>
                            <label for="fldCountry">Country: </label>
                            <select name="fldCountry" id="fldCountry" <?= ($error && !check_country($country)[0]) ? 'class="error"' : "" ?>>
                                <option value="" selected>-Select a Country-</option>
                                <?php foreach (Country::cases() as $count) : ?>
                                    <?php if ($count !== Country::unselected) : ?>
                                        <option value="<?= $count->value ?>" <?= $country === $count->value ? "selected" : "" ?>><?= $count->value ?></option>
                                    <?php endif; ?>
                                <?php endforeach; ?>
                            </select>
                            <?php if ($error && !check_country($country)[0]) : ?>
                                <span class="error">
                                    <?= check_country($country)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <section id="profSection">
                            <label for="fldProf">Professional?&nbsp;</label>
                            <input name="fldProf" id="fldProf" type="checkbox" <?= $is_professional ? "checked" : "" ?>>
                        </section>

                        <section>
                            <label for="fldImg">Image: </label>
                            <input name="fldImg" id="fldImg" type="file" accept="image/*" <?= ($error && !check_image($img)[0]) ? 'class="error"' : "" ?>>
                            <?php if ($error && !check_image($img)[0]) : ?>
                                <span class="error">
                                    <?= check_image($img)[1] ?>
                                </span>
                            <?php endif ?>
                        </section>

                        <input name="btnSubmitEdit" id="btnSubmit" type="submit">
                    </form>
                    <?php if ($error && (
                        (check_name($first_name)[0]) &&
                        (check_name($last_name)[0]) &&
                        (!str_contains($nick_name, "~")) &&
                        (check_city($city)[0]) &&
                        (check_country($country)[0]) &&
                        (check_image($img)[0])
                    )) : ?>
                        <span class="error">
                            An unexpected error occured.<br>
                            Please try again or come back later.
                        </span>
                    <?php endif ?>
                <?php else : ?>
                    <h1>Player Edited!</h1>
                    <?php
                    unset($_GET["action"]);
                    if (isset($_GET["id"])) {
                        unset($_GET["id"]);
                    }
                    ?>
                    <a href="./player-league.php" class="btnBack">Back to Player List</a>
                    <h3><?= "$first_name $last_name was successfully edited on the list." ?></h3>
                <?php endif ?>
            <?php endif ?>
        <?php endif ?>
    <?php endif ?>
</body>

</html>