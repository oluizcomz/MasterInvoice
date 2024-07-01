
-- Tabela de status

CREATE TABLE status (
    Id INT PRIMARY KEY, -- Identificador �nico do status
    Description VARCHAR(255) NOT NULL -- Descri��o do status
);
-- Tabela de notas fiscais


CREATE TABLE invoices (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador �nico da nota fiscal
    PayerName VARCHAR(255) NOT NULL, -- Nome do pagador
    IdentificationNumber VARCHAR(50) NOT NULL UNIQUE, -- N�mero de identifica��o �nico
    IssueDate DATE NOT NULL, -- Data de emiss�o da nota fiscal
    BillingDate DATE, -- Data de cobran�a
    Amount DECIMAL(10, 2) NOT NULL, -- Valor da nota fiscal
    StatusId INT, -- Identificador de status
	[InvoiceDoc] [nvarchar](255) NULL,
	[BillDoc] [nvarchar](255) NULL,
    FOREIGN KEY (StatusId) REFERENCES status(id) -- Chave estrangeira para a tabela de status
);

-- Tabela de pagamentos
CREATE TABLE payments (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificador �nico do pagamento
    InvoiceId INT, -- Identificador da nota fiscal
    PaymentDate DATE NOT NULL, -- Data do pagamento
    FOREIGN KEY (InvoiceId) REFERENCES invoices(Id) -- Chave estrangeira para a tabela de notas fiscais
);


-- �ndices para otimizar consultas
CREATE INDEX idx_issue_date ON invoices (IssueDate); -- �ndice na data de emiss�o
CREATE INDEX idx_billing_date ON invoices (BillingDate); -- �ndice na data de cobran�a
CREATE INDEX idx_status ON invoices (StatusId); -- �ndice no identificador de status


INSERT INTO status(Id,Description) VALUES (1,'Emitida');
INSERT INTO status (Id, Description) VALUES (2,'Cobran�a Realizada');
INSERT INTO status (Id, Description) VALUES (3,'Pagamento em Atraso');
INSERT INTO status (Id, Description) VALUES (4,'Pagamento Realizado');

