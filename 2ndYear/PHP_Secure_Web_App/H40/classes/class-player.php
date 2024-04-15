<?php

class Player {
    public function __construct(
        private string $first_name = "",
        private string $last_name = "",
        private string $nick_name = "",
        private int $id = -1,
        private string $city = "",
        private Country $country = Country::unselected,
        private bool $is_professional = false,
        private string|null $img = ""
    ) {
    }

    public function get_first_name(): string {
        return $this->first_name;
    }

    public function get_last_name(): string {
        return $this->last_name;
    }

    public function get_nick_name(): string {
        return $this->nick_name;
    }

    public function get_id(): int {
        return $this->id;
    }

    public function get_city(): string {
        return $this->city;
    }

    public function get_country(): Country {
        return $this->country;
    }

    public function get_is_professional(): bool {
        return $this->is_professional;
    }

    public function get_img(): string|null {
        return $this->img;
    }

    public function set_first_name(string $first_name): void {
        $this->first_name = $first_name;
    }

    public function set_last_name(string $last_name): void {
        $this->last_name = $last_name;
    }

    public function set_nick_name(string $nick_name): void {
        $this->nick_name = $nick_name;
    }

    public function set_id(int $id): void {
        $this->id = $id;
    }

    public function set_city(string $city): void {
        $this->city = $city;
    }

    public function set_country(Country $country): void {
        $this->country = $country;
    }

    public function set_is_professional(bool $professional): void {
        $this->is_professional = $professional;
    }

    public function set_img(string|null $img): void {
        $this->img = $img;
    }
}
