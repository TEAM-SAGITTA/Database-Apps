package com.sagita;

import java.util.Map;

public interface SchemaCreator {

	/**
	 * Implement this method to drop existing schema
	 */
	void dropIfExist(String[] tableNames,
			Map<String, String> fkTableMap, String[] sequence);

	/**
	 *  Implement this method to create   schema
	 */
	void create();
}
