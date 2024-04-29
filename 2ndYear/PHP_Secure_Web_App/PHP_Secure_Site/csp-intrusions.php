<?php
declare(strict_types=1);
error_reporting(E_ALL);
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);

$post_rec = file_get_contents("php://input") or die("Error: Cannot get post");
$json_rec = json_decode($post_rec, true);
$error = !empty($json_rec["csp-report"]["violated-directive"]) ? trim($json_rec["csp-report"]["violated-directive"]) : "";

if (!empty($error)) {
    $path = "./intrusion-logs.txt";
    $file = fopen($path, 'a');
	if ($file) {
		$data = ($error == "script-src-attr" ? "XSS Attempt" : "Click-Jacking Attempt") . ' | ' . date_format(new DateTime(), 'Y-m-d H:i:s') . "\n";
		fwrite($file, $data);
	}
    fclose($file);
}
