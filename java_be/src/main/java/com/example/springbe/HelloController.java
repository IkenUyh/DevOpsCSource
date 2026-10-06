package com.example.springbe;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.jdbc.core.JdbcTemplate;

import java.util.List;
import java.util.Map;

@RestController
@CrossOrigin(origins = "*")
public class HelloController {
	private final JdbcTemplate jdbcTemplate;

	public HelloController(JdbcTemplate jdbcTemplate) {
		this.jdbcTemplate = jdbcTemplate;
	}

    @GetMapping("/")
    public String hello() {
        return "Hello from Spring Boot!";
    }

	@GetMapping("/user")
	public Map<String, Object> users() {
		List<String> users = jdbcTemplate.queryForList("SELECT name FROM users", String.class);
		return Map.of("source_code", "java", "users", users);
	}
}
