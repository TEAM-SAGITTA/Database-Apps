package com.sagita;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Map;

public class OracleSchemaCreator implements SchemaCreator{

	public void dropIfExist(String[] tableNames,
			Map<String, String> fkTableMap, String[] sequence) {
		try (Connection con = DBConector.getOracleConectionInstance();) {

			if (fkTableMap != null) {
				for (String fk : fkTableMap.keySet()) {
					String tableName = fkTableMap.get(fk);
					this.dropForeignKey(tableName, fk, con);
				}
			}

			if (tableNames != null) {
				for (String table : tableNames) {
					this.dropTable(table, con);
				}
			}

			if (sequence != null) {
				for (String seqName : sequence) {
					this.dropSequence(seqName, con);
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void dropTable(String tableName, Connection con) {
		StringBuilder sb = new StringBuilder("Drop Table ").append(tableName);

		try {
			PreparedStatement ps = con.prepareStatement(sb.toString());
			ps.executeUpdate();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void dropForeignKey(String tableName, String foreignKey,
			Connection con) {
		StringBuilder dropFk = new StringBuilder("Alter Table ")
				.append(tableName).append(" Drop Constraint ")
				.append(foreignKey);
		try (PreparedStatement ps = con.prepareStatement(dropFk.toString());) {
			ps.executeUpdate();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void dropSequence(String sequenceName, Connection con) {
		StringBuilder sb = new StringBuilder("Drop sequence ")
				.append(sequenceName);
		try (PreparedStatement ps = con.prepareStatement(sb.toString())) {
			ps.executeUpdate();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void create() {
		StringBuilder queryCreateMeasureTable = new StringBuilder();
		queryCreateMeasureTable.append(" CREATE TABLE MEASURES( ");
		queryCreateMeasureTable.append("MEASURENAME VARCHAR2(50 BYTE) NOT NULL  ");
		queryCreateMeasureTable.append(", ID NUMBER(*, 0) NOT NULL   ");
		queryCreateMeasureTable.append(", CONSTRAINT MEASURES_PK PRIMARY KEY    ");
		queryCreateMeasureTable.append("( ID )");
		queryCreateMeasureTable.append("USING INDEX ");
		queryCreateMeasureTable.append("(CREATE UNIQUE INDEX MEASURES_PK ON MEASURES (ID ASC) ");
		queryCreateMeasureTable.append(" LOGGING ");
		queryCreateMeasureTable.append(" TABLESPACE USERS PCTFREE 10  INITRANS 2  STORAGE ");
		queryCreateMeasureTable.append(" (INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT )");
		queryCreateMeasureTable.append("  NOPARALLEL )  ");
		queryCreateMeasureTable.append("  ENABLE )  ");
		queryCreateMeasureTable.append("  LOGGING  TABLESPACE USERS PCTFREE 10 INITRANS 1 STORAGE   ");
		queryCreateMeasureTable.append("(  INITIAL 65536 NEXT 1048576   MINEXTENTS 1 MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT )  NOCOMPRESS ");
		
		StringBuilder queryCreateVendorsTable=new StringBuilder();
		queryCreateVendorsTable.append(" CREATE TABLE VENDORS ");
		queryCreateVendorsTable.append("(ID NUMBER(*, 0) NOT NULL , VENDORNAME VARCHAR2(50 BYTE) NOT NULL , CONSTRAINT VENDORS_PK PRIMARY KEY ( ID ) ");
		queryCreateVendorsTable.append("  USING INDEX( CREATE UNIQUE INDEX VENDORS_PK ON VENDORS (ID ASC)  LOGGING  TABLESPACE USERS   PCTFREE 10 INITRANS 2 STORAGE ");
		queryCreateVendorsTable.append("(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT )");
		queryCreateVendorsTable.append(" NOPARALLEL)  ENABLE ) LOGGING TABLESPACE USERS PCTFREE 10 INITRANS 1 STORAGE  ");
		queryCreateVendorsTable.append(" (INITIAL 65536 NEXT 1048576 MINEXTENTS 1   MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT) NOCOMPRESS  ");
		
		StringBuilder queryCreateProductTable=new StringBuilder();
		queryCreateProductTable.append(" CREATE TABLE PRODUCTS ");
		queryCreateProductTable.append("( ID NUMBER(*, 0) NOT NULL , VENDORID NUMBER(*, 0) NOT NULL , PRODUCTNAME VARCHAR2(50 BYTE) NOT NULL , MESUREID NUMBER(*, 0) NOT NULL  ");
		queryCreateProductTable.append(" , PRICE NUMBER(*, 0) NOT NULL, CONSTRAINT TABLE1_PK PRIMARY KEY(ID) ");
		queryCreateProductTable.append("USING INDEX(CREATE UNIQUE INDEX TABLE1_PK ON PRODUCTS (ID ASC)  LOGGING TABLESPACE USERS PCTFREE 10 INITRANS 2 STORAGE");
		queryCreateProductTable.append("(INITIAL 65536  NEXT 1048576 MINEXTENTS 1 MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT)NOPARALLEL)ENABLE )");
		queryCreateProductTable.append("LOGGING TABLESPACE USERS PCTFREE 10 INITRANS 1  ");
		queryCreateProductTable.append("STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS UNLIMITED BUFFER_POOL DEFAULT)NOCOMPRESS");
		
		String addProductMeasureForeignKey= "ALTER TABLE PRODUCTS ADD CONSTRAINT PRODUCT_MEASURE FOREIGN KEY(MESUREID)REFERENCES MEASURES(ID) ENABLE";
		String addProductVendorForeignKey = "ALTER TABLE PRODUCTS ADD CONSTRAINT PRODUCT_VENDOR FOREIGN KEY (VENDORID)REFERENCES MEASURES(ID) ENABLE";
		String createIdIncrementorSequence = "CREATE SEQUENCE ID_INCREMENTOR_SEQ INCREMENT BY 1 MAXVALUE 1111 MINVALUE 1 CACHE 20";
		String setIdentifiersNon ="ALTER SESSION SET PLSCOPE_SETTINGS = 'IDENTIFIERS:NONE'";
		String setIdentifiersAll ="ALTER SESSION SET PLSCOPE_SETTINGS = 'IDENTIFIERS:ALL'";
		StringBuilder createTrigerProductIdAutoIncrement = new StringBuilder("create or replace TRIGGER INCREMENT_PRODUCT_ID");
		createTrigerProductIdAutoIncrement.append(" BEFORE INSERT ON PRODUCTS");
		createTrigerProductIdAutoIncrement.append(" FOR EACH ROW ");
		createTrigerProductIdAutoIncrement.append("BEGIN :new.ID := ID_INCREMENTOR_SEQ.nextval END ");
		StringBuilder createTrigerVendorIdAutoIncrement = new StringBuilder("create or replace TRIGGER INCREMENT_VENDOR_ID");
		createTrigerVendorIdAutoIncrement.append("  BEFORE INSERT ON VENDORS");
		createTrigerVendorIdAutoIncrement.append(" FOR EACH ROW ");
		createTrigerVendorIdAutoIncrement.append("BEGIN :new.ID := ID_INCREMENTOR_SEQ.nextval END ");
		StringBuilder createTrigerMeasureIdAutoIncrement = new StringBuilder("create or replace TRIGGER INCREMENT_MEASURE_ID");
		createTrigerMeasureIdAutoIncrement.append("  BEFORE INSERT ON MEASURES");
		createTrigerMeasureIdAutoIncrement.append(" FOR EACH ROW ");
		createTrigerMeasureIdAutoIncrement.append("BEGIN :new.ID := ID_INCREMENTOR_SEQ.nextval END ");
		try (Connection con = DBConector.getOracleConectionInstance();) {
		
			
			try (PreparedStatement createMeasurePs = con.prepareStatement(queryCreateMeasureTable.toString());
					PreparedStatement createVendorPs = con.prepareStatement(queryCreateVendorsTable.toString());
					PreparedStatement createProductPs = con.prepareStatement(queryCreateProductTable.toString());
					PreparedStatement addProductMeasureForeignKeyPs = con.prepareStatement(addProductMeasureForeignKey);
					PreparedStatement addProductVendorForeignKeyPs = con.prepareStatement(addProductVendorForeignKey);
					PreparedStatement createIdIncrementorSequencePs = con.prepareStatement(createIdIncrementorSequence);
					Statement createTrigerIdAutoIncrementStatment = con.createStatement();
					PreparedStatement setIdentifiersNonPs=con.prepareStatement(setIdentifiersNon)) {
				con.setAutoCommit(false);
				
				createMeasurePs.executeUpdate();
				createVendorPs.executeUpdate();
				createProductPs.executeUpdate();
				addProductMeasureForeignKeyPs.executeUpdate();
				addProductVendorForeignKeyPs.executeUpdate();
				createIdIncrementorSequencePs.executeUpdate();
				setIdentifiersNonPs.executeUpdate();
				createTrigerIdAutoIncrementStatment.executeQuery(createTrigerProductIdAutoIncrement.toString());
				createTrigerIdAutoIncrementStatment.executeQuery(createTrigerVendorIdAutoIncrement.toString());
				createTrigerIdAutoIncrementStatment.executeQuery(createTrigerMeasureIdAutoIncrement.toString());
				createTrigerIdAutoIncrementStatment.execute(setIdentifiersAll);
				con.commit();
			} catch (SQLException e) {
				con.rollback();
				e.printStackTrace();
			}

		} catch (ClassNotFoundException | SQLException e) {
			e.printStackTrace();
		}

	}
}
