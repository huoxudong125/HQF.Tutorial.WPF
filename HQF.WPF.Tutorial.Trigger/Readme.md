#The VisualStateManager and Triggers
Silverlight introduced the Visual State Manager, which makes it easier for control template authors to specify the appearance of a control depending on its visual state.  The WPF Toolkit  ships a Visual State Manager, and the next version of WPF will include the VSM as well.   The introduction of the VisualStateManager has understandably led to questions about when  to use the VSM instead of triggers and when triggers are appropriate. This blog post attempts to address that question; more thorough discussions about how to use the VisualStateManager are elsewhere.
 
The VisualStateManager supports the parts and states model, which is a way for control authors to formalize what visual states should be in a control template.  The VisualStateManager enables control authors to manage the states of a control and provides a way for designer tools such as Microsoft Blend to support customizing the control's appearance according to its visual state.  Before the parts and states model was introduced, it was common for control template authors to use triggers to change a control's appearance when it changed visual states.  The following control template uses triggers to change the button's border color when  the mouse is over it or when it is pressed.
 
 
              <ControlTemplate TargetType="Button">
                <Grid>
                  <Ellipse Fill="{TemplateBinding BorderBrush}"/>
                  <Ellipse x:Name="ButtonShape" Margin="5" Fill="{TemplateBinding Background}"/>
                  <ContentPresenter HorizontalAlignment="Center"
                            VerticalAlignment="Center"/>
                </Grid>
 
                <ControlTemplate.Triggers>
                  <Trigger Property="IsMouseOver" Value="True">
                    <Setter Property="BorderBrush" Value="Cyan"/>
                  </Trigger>
                  <Trigger Property="IsPressed" Value="True">
                    <Setter Property="BorderBrush" Value="Red"/>
                  </Trigger>
                </ControlTemplate.Triggers>
              </ControlTemplate>
 
In this model, there is no formal agreement about what the visual states are, there are just some properties defined on a control that can be used to change the control's appearance.  A control that follows the parts and control model proactively communicates its visual states to control template authors.  When a control uses the VisualStateManager to change its visual states, it expects that the ControlTemplate uses the VisualStateManager to specify the control's appearance for a given visual state.  The control template author can also customize transitions between visual states by using VisualTransitions.  VisualTransitions enable control template authors to fine-tune an individual transition by changing the times and durations of individual property changes and even animate new properties not mentioned in either state.    The following example uses the VisualStateManager to specify the changes in the control's appearance instead of triggers.
 
              <ControlTemplate TargetType="Button">
                <Grid>
                  <VisualStateManager.VisualStateGroups>
                    <VisualStateGroup x:Name="CommonStates">
 
                    <VisualStateGroup.Transitions>
                        <!--Take one half second to transition to the MouseOver state.-->
                        <VisualTransition To="MouseOver" GeneratedDuration="0:0:0.5" />
                      </VisualStateGroup.Transitions>
 
                      <VisualState x:Name="Normal"/>
                      <VisualState x:Name="MouseOver">
                        <Storyboard>
                          <ColorAnimation Duration="0" Storyboard.TargetName="borderColor" Storyboard.TargetProperty="Color" To="Cyan"/>
                        </Storyboard>
                      </VisualState>
                      <VisualState x:Name="Pressed">
                        <Storyboard>
                          <ColorAnimation Duration="0" Storyboard.TargetName="borderColor" Storyboard.TargetProperty="Color" To="Red"/>
                        </Storyboard>
                      </VisualState>
                    </VisualStateGroup>
                  </VisualStateManager.VisualStateGroups>
                  <Ellipse>
                    <Ellipse.Fill>
                      <SolidColorBrush x:Name="borderColor" Color="Black"/>
                    </Ellipse.Fill>
                  </Ellipse>
                  <Ellipse x:Name="ButtonShape" Margin="5" Fill="{TemplateBinding Background}"/>
                  <ContentPresenter HorizontalAlignment="Center"
                            VerticalAlignment="Center"/>
                </Grid>
              </ControlTemplate>
 
The control uses the VisualStateManager to change visual states by calling VisualStateManager.GoToState.  When that occurs, the VisualStateManager appropriately stops and starts the storyboards in the VisualState and VisualTransition objects so that the button's appearance appropriately changes visual states.  Therefore, there is a distinct separation of responsibilities:  the control author specifies what the visual states of a control are and determines when a control goes into each visual state;  the template author specifies what the control looks like in each visual state.
 
Triggers aren't dead in WPF, though. You can use triggers to change the appearance of a control for properties that don’t correspond to a visual state.  For example, the Button has a IsDefault property, but there is no corresponding visual state on Button.  A control template author might want to specify the appearance of the button depending on IsDefault’s value, though, so this is a case of where using a trigger is appropriate.  The following example repeats the previous example and adds a trigger to specify the button's appearance depending on the value of IsDefault.
 
              <ControlTemplate TargetType="Button">
                <Grid>
                  <VisualStateManager.VisualStateGroups>
                    <VisualStateGroup x:Name="CommonStates">
 
                    <VisualStateGroup.Transitions>
                        <!--Take one half second to transition to the MouseOver state.-->
                        <VisualTransition To="MouseOver" GeneratedDuration="0:0:0.5" />
                      </VisualStateGroup.Transitions>
 
                      <VisualState x:Name="Normal"/>
                      <VisualState x:Name="MouseOver">
                        <Storyboard>
                          <ColorAnimation Duration="0" Storyboard.TargetName="borderColor" Storyboard.TargetProperty="Color" To="Cyan"/>
                        </Storyboard>
                      </VisualState>
                      <VisualState x:Name="Pressed">
                        <Storyboard>
                          <ColorAnimation Duration="0" Storyboard.TargetName="borderColor" Storyboard.TargetProperty="Color" To="Red"/>
                        </Storyboard>
                      </VisualState>
                    </VisualStateGroup>
                  </VisualStateManager.VisualStateGroups>
                  <Ellipse>
                    <Ellipse.Fill>
                      <SolidColorBrush x:Name="borderColor" Color="Black"/>
                    </Ellipse.Fill>
                  </Ellipse>
                  <Ellipse x:Name="defaultOutline" Stroke="{TemplateBinding Background}" StrokeThickness="2" Margin="2"/>
                  <Ellipse x:Name="ButtonShape" Margin="5" Fill="{TemplateBinding Background}"/>
                  <ContentPresenter HorizontalAlignment="Center"
                            VerticalAlignment="Center"/>
                </Grid>
                <ControlTemplate.Triggers>
                  <Trigger Property="IsDefault" Value="False">
                    <Setter TargetName="defaultOutline" Property="Stroke" Value="Transparent"/>
                  </Trigger>
                </ControlTemplate.Triggers>
              </ControlTemplate>

The IsMouseOver and IsPressed properties on Button are still available, but  you shouldn’t create triggers against them to change the button's appearance.  That doesn't mean that those properties aren't useful, though.  Those properties can still be used by application authors to check the control's visual state in code, and control authors should continue to define properties for visual states even when they use the VisualStateManager to transition between their visual states.
