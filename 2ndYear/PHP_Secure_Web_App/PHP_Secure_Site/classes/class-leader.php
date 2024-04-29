<?php

class Leader {
    public function __construct(
        private string $email = "",
        private string $password = "",
        private string $first_name = "",
        private string $last_name = ""
    ) {
    }

    public function get_email(): string {
        return $this->email;
    }

    public function get_password(): string {
        return $this->password;
    }

    public function get_first_name(): string {
        return $this->first_name;
    }

    public function get_last_name(): string {
        return $this->last_name;
    }

    public function set_email(string $email): void {
        $this->email = $email;
    }

    public function set_password(string $password): void {
        $this->password = $password;
    }

    public function set_first_name(string $first_name): void {
        $this->first_name = $first_name;
    }

    public function set_last_name(string $last_name): void {
        $this->last_name = $last_name;
    }
}
