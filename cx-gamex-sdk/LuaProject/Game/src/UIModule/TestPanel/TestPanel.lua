

TestPanel = {}

function TestPanel:Create()
    TestPanel.panel = UIManager:Open("TestPanel")
    TestPanel.btn = TestPanel.panel.transform:Find("btn"):GetComponent("Button")

    TestPanel.btn.onClick:AddListener(function ()
        UIManager:Close("TestPanel")

        Utils:ToGame("Game")
        print("<color=red>101</color>")

    end)

    print("<color=red>TestPanel</color>")
end





return TestPanel