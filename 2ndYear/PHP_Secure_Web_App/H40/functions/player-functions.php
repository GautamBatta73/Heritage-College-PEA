<?php
function check_name(string $name): array {
    $pattern = '/^[a-z][(a-z|\-|\'|\s)]*[a-z]$/i';
    $arr = [true, ""];

    if (empty($name)) {
        $arr = [false, "This Name is required"];
    } else if (!preg_match($pattern, $name)) {
        if (!preg_match("/^[a-z]/i", $name) || !preg_match("/[a-z]$/i", $name)) {
            $arr = [false, "This Name must start and end with a letter"];
        } else if (preg_match("/[^a-z\-\'\s)]/i", $name)) {
            $arr = [false, "This Name must only contain letters, dashes, apostrophes, and spaces"];
        } else {
            $arr = [false, "Unknown error"];
        }
    }

    return $arr;
}

function check_city(string $city): array {
    $pattern = '/^[a-z][(a-z|\-|\s)]*[a-z]$/i';
    $arr = [true, ""];

    if (empty($city)) {
        $arr = [false, "City is required"];
    } else if (!preg_match($pattern, $city)) {
        if (!preg_match("/^[a-z]/i", $city) || !preg_match("/[a-z]$/i", $city)) {
            $arr = [false, "City must start and end with a letter"];
        } else if (preg_match("/[^a-z\-\s)]/i", $city)) {
            $arr = [false, "City must only contain letters, dashes, and spaces"];
        } else {
            $arr = [false, "Unknown error"];
        }
    }

    return $arr;
}

function check_image(string|null $img): array {
    if ($img === null) {
        $img = "";
    }
    $arr = [false, "Uploaded file must be a valid image file (png, jpg, etc)"];
    $img = strtolower($img);
    $formats = [
        ".apng",
        ".avif",
        ".gif",
        ".jpeg",
        ".jpg",
        ".png",
        ".svg",
        ".webp"
    ];

    if (!empty($img)) {
        if ($_FILES["fldImg"]["size"] > 10000000) {
            $arr = [false, "Uploaded file must be less than 10mb"];
        } else {
            foreach ($formats as $ext) {
                if (str_ends_with($img, $ext)) {
                    $arr = [true, ""];
                }
            }
        }
    } else {
        $arr = [true, ""];
    }

    return $arr;
}

function check_country(string $country): array {
    $arr = [false, "Country is invalid"];
    $country = ucwords(strtolower($country));
    $valid_countries = [];
    foreach (Country::cases() as $count) {
        if ($count !== Country::unselected) {
            array_push($valid_countries, $count->value);
        }
    }

    if (!empty($country)) {
        foreach ($valid_countries as $valid_country) {
            if ($country === $valid_country) {
                $arr = [true, ""];
            }
        }
    } else {
        $arr = [false, "Country is required"];
    }

    return $arr;
}

function check_id(string $id): array {
    $arr = [true, ""];

    if (!is_numeric($id)) {
        $arr = [false, "ID must be a number"];
    } else {
        if ((int)$id <= 0) {
            $arr = [false, "ID must be more than 0"];
        } else {
            $path = './data/players.txt';
            if (file_exists($path)) {
                $temp_arr = file($path);
                $split_arr = [];
                foreach ($temp_arr as $value) {
                    $temp_val = explode("~", $value);
                    array_push($split_arr, $temp_val);
                }
                if (in_array($id, array_column($split_arr, 0))) {
                    $arr = [false, "ID already exists"];
                }
            }
        }
    }
    return $arr;
}

function add_player(Player $player, string $email): bool {
    $dest = './data/players.txt';
    $file = fopen($dest, 'a');
    $data = $player->get_id() . "~" . $player->get_first_name() . "~" . $player->get_last_name() . "~" . $player->get_nick_name() . "~" . $player->get_city() . "~" . $player->get_country()->name . "~" . (($player->get_is_professional()) ? "yes" : "no") . "~" . strtolower($email) . "\n";

    fwrite($file, $data);
    fclose($file);

    if (count($_FILES) > 0 && (file_exists($_FILES['fldImg']['tmp_name']) && is_uploaded_file($_FILES['fldImg']['tmp_name']))) {
        $tempFilePath = $_FILES['fldImg']['tmp_name'];
        $destFilePath = "./player-images/player" . $player->get_id() . "." . pathinfo($_FILES['fldImg']['name'], PATHINFO_EXTENSION);

        return (move_uploaded_file($tempFilePath, $destFilePath));
    }

    return true;
}
