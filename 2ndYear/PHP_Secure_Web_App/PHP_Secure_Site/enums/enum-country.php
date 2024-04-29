<?php
enum Country: string {
    case unselected = "";
    case canada = "Canada";
    case usa = "United States";
    case argentina = "Argentina";
    case brazil = "Brazil";
    case columbia = "Columbia";
    case ecuador = "Ecuador";
    case peru = "Peru";
    case poland = "Poland";

    public static function from_name(string $name): Country {
        $country_found = Country::unselected;
        foreach (self::cases() as $country) {
            if (strtolower($name) === strtolower($country->name)) {
                $country_found = $country;
            }
        }

        return $country_found;
    }

    public static function from_val(string $val): Country {
        $country_found = Country::unselected;
        foreach (self::cases() as $country) {
            if (strtolower($val) === strtolower($country->value)) {
                $country_found = $country;
            }
        }

        return $country_found;
    }
}
