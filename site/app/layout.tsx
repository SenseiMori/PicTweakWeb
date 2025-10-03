import Layout, { Content, Footer, Header } from "antd/es/layout/layout";
import "./globals.css";
import { Menu } from "antd";
import Link from "next/link";



const items = [
    {
        key: "���������", label: <Link href="/mod">���������</Link>,
    },
    {
        key: "������ �������", label: <Link href="/profile">������ �������</Link>,
    },
    {
        key: "���������", label: <Link href="/settings">���������</Link>,
    },
];

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
      <html lang="en">
      <body>
              <Layout style={{ minHeight: "100vh", display: "flex" }}>
                  <Header>
                      <Menu theme="dark"
                          mode="horizontal"
                          items={items}
                          style={{ flex: 1, minWidth: 0 }}
                          />
                  </Header>
                  <Content style={{ padding: "0 48px" }}> {children} </Content>
                  <Footer style={{ textAlign: "center" }}>
                      Ant Design �2023 Created by Ant UED
                  </Footer>
              </Layout>
      </body>
    </html>
  );
}
