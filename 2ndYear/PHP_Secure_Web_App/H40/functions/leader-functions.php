<?php
function valid_pass(string $password): array {
    $pattern = '/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{8,16}$/';
    $arr = [true, ""];

    if (empty($password)) {
        $arr = [false, "Password is required"];
    } else if (str_contains($password, "~")) {
        $arr = [false, "Password can not contain '~'"];
    } else if (!preg_match($pattern, $password)) {
        if (strlen($password) < 8 || strlen($password) > 16) {
            $arr = [false, "Password must be 8-16 characters long"];
        } else if (!preg_match("/[\d]/", $password)) {
            $arr = [false, "Password must contain a number"];
        } else if (!preg_match("/[A-Z]/", $password)) {
            $arr = [false, "Password must contain a uppercase letter"];
        } else if (!preg_match("/[a-z]/", $password)) {
            $arr = [false, "Password must contain an lowercase letter"];
        } else if (preg_match("/\s/", $password)) {
            $arr = [false, "Password must not contain spaces"];
        } else if (!preg_match("/[^a-zA-Z0-9]+/", $password)) {
            $arr = [false, "Password must contain special characters"];
        } else {
            $arr = [false, "Unknown error"];
        }
    }

    return $arr;
}

function valid_email(string $email): array {
    $pattern = '/^[a-z][^@]*[@][^@]+\.[a-z]{2,}$/i';
    $arr = [true, ""];

    if (empty($email)) {
        $arr = [false, "Email is required"];
    } else if (str_contains($email, "~")) {
        $arr = [false, "Email can not contain '~'"];
    } else if (!preg_match($pattern, $email)) {
        if (!preg_match("/^[^@]*@[^@]*$/", $email)) {
            $arr = [false, "Email must contain only one '@'"];
        } else if (!preg_match("/^[a-z].+[a-z]$/i", $email)) {
            $arr = [false, "Email must start and end with a letter"];
        } else if (!preg_match("/.+[@]/", $email)) {
            $arr = [false, "Email must start and end with a letter"];
        } else if (!preg_match("/[@].+\.[a-z]{2,}/", $email)) {
            $arr = [false, "Email must have a domain after the '@' and a top-level domain of at least 2 characters"];
        }
    }

    return $arr;
}

function check_email(string $email): array {
    $arr = [true, ""];

    if (valid_email($email)[0]) {
        $path = './data/leader-data.txt';
        if (file_exists($path)) {
            $temp_arr = file($path);
            $split_arr = [];
            foreach ($temp_arr as $value) {
                $temp_val = explode("~", $value);
                array_push($split_arr, $temp_val);
            }
            if (in_array($email, array_column($split_arr, 0))) {
                $arr = [false, "Email already exists"];
            }
        }
    } else {
        $arr = valid_email($email);
    }
    return $arr;
}

function confirm_password(string $password, string $hash): array {
    $arr = [false, "Password do not match"];
    if (password_verify($password, $hash)) {
        $arr = [true, ""];
    }
    return $arr;
}

function valid_login(string $email, string $password): array {
    $email = strtolower($email);
    $arr = [false, "Email or Password is Incorrect."];

    $path = './data/leader-data.txt';
    if (!file_exists(dirname($path))) {
        mkdir(dirname($path), 0755, true);
        touch($path);
    }

    $file = fopen($path, "r");
    if ($file) {
        while (($temp_ln = fgets($file)) !== false) {
            $temp = explode("~", $temp_ln);
            $temp_email = strtolower(trim($temp[0]));
            $temp_password = trim($temp[1]);

            if ($email === $temp_email && confirm_password($password, $temp_password)[0]) {
                $arr = [true, ""];
            }
        }
        fclose($file);
    }
    return $arr;
}

function get_leader(string $email): Leader|null {
    $email = strtolower($email);
    $leader = null;

    $path = './data/leader-data.txt';
    if (!file_exists(dirname($path))) {
        mkdir(dirname($path), 0755, true);
        touch($path);
    }

    $file = fopen($path, "r");
    if ($file) {
        while (($temp_ln = fgets($file)) !== false) {
            $temp = explode("~", $temp_ln);
            $temp_email = strtolower(trim($temp[0]));

            if ($email === $temp_email) {
                $leader = new Leader($temp[0], $temp[1], $temp[2], $temp[3]);
            }
        }
        fclose($file);
    }

    return $leader;
}


function add_leader(Leader $leader): void {
    $dest = './data/leader-data.txt';
    $file = fopen($dest, 'a');
    if (!file_exists(dirname($dest))) {
        mkdir(dirname($dest), 0755, true);
        touch($dest);
    }
    $data = $leader->get_email() . "~" . $leader->get_password() . "~" . $leader->get_first_name() . "~" . $leader->get_last_name() . "\n";

    fwrite($file, $data);
    fclose($file);
}

function set_players(string $email): array | null {
    $arr = null;
    $email = strtolower($email);

    $path = './data/players.txt';
    if (!file_exists(dirname($path))) {
        mkdir(dirname($path), 0755, true);
        touch($path);
        touch("./data/leader-data.txt");
    }
    if (!file_exists("./player-images/")) {
        mkdir("./player-images/", 0755, true);
        copy('./images/default-player-image.png', './player-images/default.png');
    }
    $file = fopen($path, "r");
    if ($file) {
        $arr = [];
        while (($temp_ln = fgets($file)) !== false) {
            $temp = explode("~", $temp_ln);
            if (strtolower(trim($temp[7])) === $email) {
                $temp[0] = trim($temp[0]);
                if (is_numeric($temp[0])) {
                    $playerimg = glob("./player-images/player" . $temp[0] . ".*");
                    $player = new Player($temp[1], $temp[2], $temp[3], (int)$temp[0], $temp[4], Country::from_name($temp[5]), (trim($temp[6]) === "yes"));

                    if (count($playerimg) > 0) {
                        $player->set_img($playerimg[0]);
                    } else {
                        $player->set_img(null);
                    }

                    array_push($arr, $player);
                }
            }
        }
        fclose($file);

        if (count($arr) > 0) {
            usort($arr, function ($a, $b) {
                if ($a->get_last_name() == $b->get_last_name()) {
                    return $a->get_first_name() <=> $b->get_first_name();
                }
                return $a->get_last_name() <=> $b->get_last_name();
            });
        } else {
            $arr = null;
        }
    }

    return $arr;
}

function get_player_index(array $player_list, string $id, string $email): int {
    if (is_array($player_list[0])) {
        foreach ($player_list as $index => $player) {
            if ($player[0] === $id && strtolower(trim($player[7])) === $email) {
                return $index;
            }
        }
    } else {
        foreach ($player_list as $index => $player) {
            if ($player->get_id() === (int)$id) {
                return $index;
            }
        }
    }
    return -1;
}

function delete_player(string $id, string $email): void {
    $path = './data/players.txt';
    $email = strtolower($email);

    if (is_numeric($id)) {
        $players = file($path);
        if (count($players) > 0) {
            $arr = [];
            foreach ($players as $player) {
                array_push($arr, explode("~", $player));
            }
            $i = get_player_index($arr, $id, $email);
            if ($i > -1) {
                unset($arr[$i]);
                $arr = array_values($arr);

                usort($arr, fn ($a, $b) => $a[0] <=> $b[0]);

                $path = './data/players.txt';
                $file = fopen($path, 'w');

                foreach ($arr as $play) {
                    $data = $play[0] . "~" . $play[1] . "~" . $play[2] . "~" . $play[3] . "~" . $play[4] . "~" . $play[5] . "~" . $play[6] . "~" . $play[7];
                    fwrite($file, $data);
                }

                fclose($file);

                $playerimg = glob("./player-images/player" . $id . ".*");

                if (count($playerimg) > 0) {
                    unlink($playerimg[0]);
                }
            }
        }
    }
}
